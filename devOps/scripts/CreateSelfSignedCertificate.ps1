param (
    [string]$DnsName,
    [string]$OutputDirectory,
    [System.Security.SecureString]$Password,
    [int]$ValidityInYears = 1
)

function PromptForInput {
    param (
        [string]$PromptMessage,
        [bool]$IsSecure = $false
    )
    if ($IsSecure) {
        return Read-Host -Prompt $PromptMessage -AsSecureString
    } else {
        return Read-Host -Prompt $PromptMessage
    }
}

function Test-Administrator {
    $currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
    $currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

function Start-Elevated {
    param (
        [string]$ScriptPath
    )
    Start-Process powershell -ArgumentList "-ExecutionPolicy Bypass -File `"$ScriptPath`"" -Verb RunAs
}

if (-not (Test-Administrator)) {
    Write-Warning "This script must be run as an administrator."
    $response = Read-Host "Do you want to elevate privileges and run this script as an administrator? (Y/N)"
    if ($response -eq 'Y' -or $response -eq 'y') {
        Start-Elevated -ScriptPath $PSCommandPath
        exit
    } else {
        Write-Output "Please run the following command with elevated privileges:"
        Write-Output "powershell -ExecutionPolicy Bypass -File `"$PSCommandPath`""
        exit
    }
}

# Set the execution policy to Bypass for the current process
Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process -Force

if (-not $DnsName) {
    $DnsName = PromptForInput "Enter the DNS name for the certificate"
}

if (-not $OutputDirectory) {
    $OutputDirectory = PromptForInput "Enter the output directory for the files"
}

if (-not $Password) {
    $Password = PromptForInput "Enter the password for the .pfx file" $true
}

# Convert DnsName to uppercase
$DnsNameUpper = $DnsName.ToUpper()

# Create full paths for the .pfx, Base64, thumbprint, and instructions files
$PfxPath = Join-Path -Path $OutputDirectory -ChildPath "$DnsName.pfx"
$Base64Path = Join-Path -Path $OutputDirectory -ChildPath "$DnsName.pfx.b64"
$ThumbprintPath = Join-Path -Path $OutputDirectory -ChildPath "$DnsName.thumbprint.txt"
$InstructionsPath = Join-Path -Path $OutputDirectory -ChildPath "$DnsName.instructions.md"
$ZipPath = Join-Path -Path $OutputDirectory -ChildPath "$DnsName.signing.zip"

# Convert the secure string password to a plain text string
$Ptr = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($Password)
$PasswordPlainText = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($Ptr)

# Create the self-signed certificate
$cert = New-SelfSignedCertificate -Type CodeSigningCert -DnsName $DnsName -CertStoreLocation "Cert:\CurrentUser\My" -NotAfter (Get-Date).AddYears($ValidityInYears)

# Export the certificate as a .pfx file
$passwordSecureString = ConvertTo-SecureString -String $PasswordPlainText -Force -AsPlainText
Export-PfxCertificate -Cert $cert -FilePath $PfxPath -Password $passwordSecureString

# Output the thumbprint
$thumbprint = $cert.Thumbprint
Write-Output "Certificate Thumbprint: $thumbprint"

# Save the thumbprint to a text file
$thumbprint | Out-File -FilePath $ThumbprintPath
Write-Output "Thumbprint saved to: $ThumbprintPath"

# Create instructions file
$instructions = @"
## Next Steps

1. **Update the Project File:**
   - Open your project file (.csproj).
   - Update the `<PackageCertificateThumbprint>` property with the following thumbprint:
     ```
     $thumbprint
     ```

2. **Add the Certificate to GitHub Secrets:**
   - Go to your GitHub repository.
   - Navigate to `Settings > Secrets and variables > Actions > New repository secret`.
   - Add a new secret named BASE64_ENCODED_PFX and paste the contents of the `$Base64Path` file.
   - Add another secret named PFX_PASSWORD and use the password you provided for the .pfx file.

3. **Configure GitHub Actions Workflow:**
   - Ensure your GitHub Actions workflow file references the secrets BASE64_ENCODED_PFX and PFX_PASSWORD.

4. **Import the Certificate:**
   - Import the certificate (.cer file) into the Trusted Root Certification Authorities store on your development machine.

## Files Created

- `$PfxPath`
- `$Base64Path`
- `$ThumbprintPath`
- `$InstructionsPath`

"@
$instructions | Out-File -FilePath $InstructionsPath
Write-Output "Instructions saved to: $InstructionsPath"

# Check if the .pfx file was created successfully
if (Test-Path $PfxPath) {
    # Convert the .pfx file to Base64 and save it
    [Convert]::ToBase64String([System.IO.File]::ReadAllBytes($PfxPath)) | Out-File -FilePath $Base64Path
    Write-Output "Base64 encoded .pfx file saved to: $Base64Path"
} else {
    Write-Error "Failed to create the .pfx file at $PfxPath"
    exit 1
}

# Export the certificate as a .cer file for importing into Trusted Root Certification Authorities
$cerPath = [System.IO.Path]::ChangeExtension($PfxPath, ".cer")
Export-Certificate -Cert $cert -FilePath $cerPath

# Import the certificate to the Trusted Root Certification Authorities
Import-Certificate -FilePath $cerPath -CertStoreLocation Cert:\LocalMachine\Root

# Import the .pfx file into the certificate store
Import-PfxCertificate -FilePath $PfxPath -CertStoreLocation Cert:\CurrentUser\My -Password $passwordSecureString

# Zip all the generated files
$filesToZip = @($PfxPath, $Base64Path, $ThumbprintPath, $InstructionsPath, $cerPath)
Compress-Archive -Path $filesToZip -DestinationPath $ZipPath -Force
Write-Output "All files have been zipped into: $ZipPath"

# Delete the individual files after zipping
foreach ($file in $filesToZip) {
    Remove-Item -Path $file -Force
}

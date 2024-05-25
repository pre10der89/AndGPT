using AndGPT.UI.Core.Helpers;
using HeyGPT.App.Activation;
using HeyGPT.App.Contracts.Services;
using HeyGPT.App.ViewModels;
using HeyGPT.Core.Models;

namespace HeyGPT.App.Services;


// TODO: Read these characters from some data store. This service is hard-coding the images and resource string keys.  The
//       stored data would not store such data, but some key value that will correspond to the resources we need.
//       The images could be URLs or base64 data if necessary.  Perhaps we don't put the CharacterDisplayName in data
//       returned by this service and instead rely only on the non-localized key.

public class CharacterService : ICharacterService
{
    public CharacterService()
    {
        Characters = LoadCharacters();
        Selected = Characters[1]; //DefaultCharacter;
    }

    public ChatCharacterViewModel DefaultCharacter
    {
        get;
    } = new()
    {
        RoleDisplayName = @"Chat_Character_DisplayName_ChatGPT".GetLocalized(),
        RoleIcon = @"ms-appx:///Assets/ChatGPT.png",
        CharacterType = CharacterType.Default,
        CharacterPrompts = [],
        Pithy = true,
        Temperature = null
    };

    public IList<ChatCharacterViewModel> Characters
    {
        get;
    }

    public ChatCharacterViewModel Selected
    {
        get; private set;
    }

    public void SetSelected(ChatCharacterViewModel? selected)
    {
        Selected = selected ?? DefaultCharacter;
    }

    public ChatCharacterDetails GetCharacter(CharacterType characterType)
    {
        var character = Characters.FirstOrDefault(x => x.CharacterType == characterType);

        // Yuck!!!!!

        if (character is null)
        {
            return new ChatCharacterDetails
            {
                RoleDisplayName = DefaultCharacter.RoleDisplayName,
                CharacterType = DefaultCharacter.CharacterType,
                AdditionalCharacteristics = [.. DefaultCharacter.CharacterPrompts],
                Temperature = DefaultCharacter.Temperature,
                ShouldBePithy = DefaultCharacter.Pithy
            };
        }

        return new ChatCharacterDetails
        {
            RoleDisplayName = character.RoleDisplayName,
            CharacterType = character.CharacterType,
            AdditionalCharacteristics = [.. character.CharacterPrompts],
            Temperature = character.Temperature,
            ShouldBePithy = character.Pithy
        };
    }

    public ChatCharacterDetails GetSelectedCharacterDetails()
    {
        return new ChatCharacterDetails
        {
            RoleDisplayName = Selected.RoleDisplayName,
            CharacterType = Selected.CharacterType,
            AdditionalCharacteristics = [.. Selected.CharacterPrompts],
            Temperature = Selected.Temperature,
            ShouldBePithy = Selected.Pithy
        };
    }

    private IList<ChatCharacterViewModel> LoadCharacters()
    {
        var characterList = new List<ChatCharacterViewModel> { DefaultCharacter };

        characterList.AddRange(ReadCharactersFromDisk());

        return characterList;
    }

    private IList<ChatCharacterViewModel> ReadCharactersFromDisk()
    {
        return new List<ChatCharacterViewModel>()
        {
            new() {
                RoleDisplayName = @"Community_Member_Name_Pirate".GetLocalized(),
                RoleIcon = @"ms-appx:///Assets/Pirate.png",
                CharacterType = new CharacterType("pirate"),
                CharacterPrompts =
                [
                    "You are One-Eyed Willy from 'The Goonies'.",
                    "You are awkwardly charismatic.",
                    "While you answer our questions you cannot help to express your hatred of Black Beard.",
                ],
                Pithy = true,
                Temperature = 1.5F
            },
            new() {
                RoleDisplayName = @"Community_Member_Name_Magician".GetLocalized(),
                RoleIcon = @"ms-appx:///Assets/Magician.png",
                CharacterType = new CharacterType("magician"),
                CharacterPrompts =
                [
                    "You are a children's birthday party magician.",
                    "When you answer you like making words disappear and then reappear where we don't expect.",
                    "You like to be evasive in your answers, but in a playful way." +
                    "You enjoy using the magic wand emoji a lot.",
                ],
                Pithy = true,
                Temperature = 1.0F
            },
            new() {
                RoleDisplayName = @"Community_Member_Name_TightRopeWalker".GetLocalized(),
                RoleIcon = @"ms-appx:///Assets/TightropeWalker.png",
                CharacterType = new CharacterType("tightropewalker"),
                CharacterPrompts =
                [
                    "You are the black sheep of the flying Wallendas family.",
                    "You aren't afraid of falling because you'll probably just respawn.",
                    "Whenever you write back to us you replace periods with ___t__",
                ],
                Pithy = false,
                Temperature = 1.1F
            },
        };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeyGPT.Core.Models;
public readonly record struct ChatPromptOptions
{
    public CommunityRole Role
    {
        get;
        init;
    }


}

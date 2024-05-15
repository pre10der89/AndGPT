using HeyGPT.App.ViewModels;
using HeyGPT.Core.Models;

namespace HeyGPT.App.Contracts.Services;

public interface ICharacterService
{
    ChatCharacterViewModel DefaultCharacter { get; }

    IList<ChatCharacterViewModel> Characters { get; }

    ChatCharacterViewModel Selected { get; }

    void SetSelected(ChatCharacterViewModel selected);


    ChatCharacterDetails GetSelectedCharacterDetails();
}

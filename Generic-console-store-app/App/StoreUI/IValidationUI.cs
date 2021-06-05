using System;

namespace StoreUI
{
    public interface IValidationUI
    {
        string ValidationPrompt(string prompt, Func<string, bool> validate);
    }
}
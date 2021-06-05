using System;

namespace UI
{
    public interface IValidationUI
    {
        string ValidationPrompt(string prompt, Func<string, bool> validate);
    }
}
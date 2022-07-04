using System.Collections.Generic;

namespace ServerService.Essential.AdminClient.Events
{
    internal interface IEventsOriginator<TRule, TInput>
    {
        void Create(TInput inputArgument);
        void ReloadRules();
        List<TRule> GetRules();
    }
}

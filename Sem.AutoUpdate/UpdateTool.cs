namespace Sem.AutoUpdate
{
    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Interfaces;

    public class UpdateTool
    {
        public void UpdateMe(IUiInteraction uiProvider)
        {
            var updateNeeded = new VersionCheck().Check(uiProvider);
            
            if (updateNeeded)
            {
                
            }
        }
    }
}

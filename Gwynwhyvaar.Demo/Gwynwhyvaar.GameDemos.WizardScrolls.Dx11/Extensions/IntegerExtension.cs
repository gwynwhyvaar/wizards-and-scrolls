namespace Gwynwhyvaar.GameDemos.WizardScrolls.Dx11.Extensions
{
    public static class IntegerExtension
    {
        public static int GetPreviousLevel(this int level)
        {
            if (level > 1)
            {
                return level - 1;
            }
            return 1;
        }
    }
}

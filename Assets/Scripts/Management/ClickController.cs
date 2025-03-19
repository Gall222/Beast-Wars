namespace Game.Management
{
    public class ClickController
    {
        public ClickTypes currentClick;

        public enum ClickTypes
        {
            None,
            Skill,
            Menu,
        }
    }
}
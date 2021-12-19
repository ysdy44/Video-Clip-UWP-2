namespace Video_Clip2.Medias
{
    public abstract class Media
    {

        //@Property
        public string Name { get; protected set; }
        public string FileType { get; protected set; }
        public string Token { get; protected set; }

        public Medium ToMedium()
        {
            return new Medium
            {
                Name = this.Name,
                FileType = this.FileType,
                Token = this.Token,
            };
        }

    }
}
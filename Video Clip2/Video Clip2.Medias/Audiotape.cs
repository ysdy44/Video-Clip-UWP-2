﻿namespace Video_Clip2.Medias
{
    public struct Audiotape
    {
        public string Name;
        public string FileType;
        public string Token;

        //@Override
        public override string ToString() => this.Token;

    }
}
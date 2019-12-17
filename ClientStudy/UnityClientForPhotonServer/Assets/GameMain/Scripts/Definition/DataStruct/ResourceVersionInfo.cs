namespace StarForce
{
    public class ResourceVersionInfo
    {

        public string LocalApplicableGameVersion
        {
            get;
            set;
        }

        public int LocalInternalResourceVersion
        {
            get;
            set;
        }

        public string LatestApplicableGameVersion
        {
            get;
            set;
        }

        public int LatestInternalResourceVersion
        {
            get;
            set;
        }

        public int Length
        {
            get;
            set;
        }

        public int HashCode
        {
            get;
            set;
        }

        public int ZipLength
        {
            get;
            set;
        }

        public int ZipHashCode
        {
            get;
            set;
        }
    }
}

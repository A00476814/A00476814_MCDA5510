namespace aplha{
    class DirWalker
    {
        public static string[]  getFilePathList(string rootPath)
        {

            string[] filePathList = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
            return filePathList;
        }
    }
}

namespace TextHeaderAnalyzerFrameProj
{
    public static class StringExtensions
    {
        public static int GetNumberOfTabs(this string input)
        {
            int i = 0;
            foreach (var c in input)
            {
                if (c == '\t')
                {
                    i++;
                }
                else
                {
                    return i;
                }
            }

            return i;
        }

        public static int GetIsHeaderLevel(this string input)
        {
            //Todo write test for this method
            int i = 0, lvl = 1;
            if (input.Length >= 2)
            {
                foreach (var c in input)
                {
                    if (c == '\t')
                    {
                        lvl++;
                    }
                    else
                    {
                        if (input[i] == '/' && input[i + 1] == '/')
                        {
                            return lvl;
                        }

                        return 0;
                    }

                    i++;
                }
            }

            return 0;
        }
    }
}

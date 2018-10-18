using System;
namespace FXGame
{
    public class GameVersion : Object
    {
        //当前版本
        public static GameVersion currentVersion;
        //目标版本
        public static GameVersion targetVersion;
        //当前安装包的安装版本
        public static GameVersion installVersion;

        //程序版本
        private Version m_appVersion = new Version("0.0.0");
        public System.Version AppVersion
        {
            get { return m_appVersion; }
            set { m_appVersion = value; }
        }

        //lua脚本版本
        private Version m_luaVersion = new Version("0.0.0");
        public System.Version LuaVersion
        {
            get { return m_luaVersion; }
            set { m_luaVersion = value; }
        }
        //资源包版本
        private Version m_resVersion = new Version("0.0.0");
        public System.Version ResVersion
        {
            get { return m_resVersion; }
            set { m_resVersion = value; }
        }

        //服务器版本
        private Version m_serverVersion = new Version("1.0.0.0");
        public System.Version ServerVersion
        {
            get { return m_serverVersion; }
            set { m_serverVersion = value; }
        }

        public void SetVersionTxt(string str)
        {
            string[] sps = str.Split('\n');
            if (sps.Length < 3) return;
            m_appVersion = new Version(sps[0]);
            m_luaVersion = new Version(sps[1]);
            m_resVersion = new Version(sps[2]);
            m_serverVersion = new Version(sps.Length > 3 ? sps[3] : "1.0.0.0");
        }



        public string GetVersionTxt()
        {
            return m_appVersion.ToString() + "\r\n" + m_luaVersion.ToString() + "\r\n" + m_resVersion.ToString() + "\r\n" + m_serverVersion.ToString();
        }

        public string GetVersionTxtEx()
        {
            return "安装版本：" + (null != m_appVersion ? m_appVersion.ToString() : "") + "\n"
                + "脚本版本：" + (null != m_luaVersion ? m_luaVersion.ToString() : "") + "\n"
                + "资源版本：" + (null != m_resVersion ? m_resVersion.ToString() : "") + "\n"
                + "服务器版本：" + (null != m_serverVersion ? m_serverVersion.ToString() : "");
        }

        public void IterateAppVer()
        {
            m_appVersion = VerIterate(m_appVersion);
        }

        public void IterateLuaVer()
        {
            m_luaVersion = VerIterate(m_luaVersion);
        }

        public void IterateResVer()
        {
            m_resVersion = VerIterate(m_resVersion);
        }

        public void IterateServerVer()
        {
            m_serverVersion = VerIterate(m_serverVersion);
        }

        static int[] maxnums = new int[4] { 99, 99, 999, 999 };
        /// <summary>
        /// 版本迭代
        /// </summary>
        /// <param name="ver"></param>
        /// <returns></returns>
        public static Version VerIterate(Version ver)
        {
            //const int vlen = 3;
            int vlen = ver.ToString().Split('.').Length;
            int[] nums = new int[] { ver.Major, ver.Minor, ver.Build, ver.Revision };
            string verstr = "";
            for (int i = vlen - 1; i >= 0; i--)
            {
                if (i == vlen - 1) nums[i]++;

                if (nums[i] > maxnums[i])
                {
                    nums[i] = 0;
                    if (i != 0) nums[i - 1]++;
                }

                verstr = nums[i].ToString() + verstr;
                if (i != 0) verstr = "." + verstr;

            }
            return new Version(verstr);
        }


        public bool Eq(GameVersion v2)
        {
            return this.AppVersion == v2.AppVersion && this.LuaVersion == v2.LuaVersion && this.ResVersion == v2.ResVersion;
        }
    }
}

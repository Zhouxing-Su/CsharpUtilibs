using System.Collections.Generic;


namespace IDeal.Szx.CsharpUtilibs.OS
{
    using IDeal.Szx.CsharpUtilibs.Collections;


    /// <summary>
    /// it can handles 3 types of arguments.                               <para />
    /// the first one is plain arguments, which come by themselves.        <para />
    /// the second one is map arguments, which come as key value pairs.
    ///     (the keys are predefined, one value for one key)               <para />
    /// the third one is switch arguments, which come by themselves.
    ///     (the values are predefined)
    /// </summary>
    /// <remarks> if there are plain arguments only, this class is useless. </remarks>
    public class ArgsProcessor
    {
        #region Constructor
        #endregion Constructor

        #region Method
        /// <summary> reset processor state and organize arguments. </summary>
        /// <param name="args"> arguments to be handled. </param>
        /// <param name="options"> pre-defined key of map arguments. </param>
        /// <param name="switchs"> pre-defined value of switch arguments. </param>
        public void Process(string[] args, string[] options = null, string[] switchs = null) {
            if (args == null) { return; }

            Clear();

            if (options != null) {
                foreach (string item in options) { mapArgs.Add(item, null); }
            }
            HashSet<string> switchSet = ((switchs == null)
                ? (new HashSet<string>()) : (new HashSet<string>(switchs)));

            for (int i = 0; i < args.Length; i++) {
                if (mapArgs.ContainsKey(args[i])) {
                    mapArgs[args[i]] = args[i + 1];
                    i++;
                } else if (switchSet.Contains(args[i])) {
                    switchArgs.Add(args[i]);
                } else {
                    plainArgs.Add(args[i]);
                }
            }
        }

        /// <summary> remove all stored arguments. </summary>
        public void Clear() {
            plainArgs.Clear();
            mapArgs.Clear();
            switchArgs.Clear();
        }

        /// <summary> get a plain argument by order of appeared plain args. </summary>
        public string GetPlainArg(int argIndex) {
            return plainArgs[argIndex];
        }

        /// <summary> get a map argument by key. </summary>
        public string GetMappedArg(string optionName) {
            return mapArgs[optionName];
        }

        /// <summary> if the switch is specified, return true. </summary>
        public bool GetSwitchArg(string switchName) {
            return switchArgs.Contains(switchName);
        }
        #endregion Method

        #region Property
        public IReadOnlyList<string> PlainArgs {
            get { return plainArgs; }
        }

        public IReadOnlyDictionary<string, string> MapArgs {
            get { return mapArgs; }
        }

        public ReadOnlySet<string> SwitchArgs {
            get { return new ReadOnlySet<string>(switchArgs); }
        }
        #endregion Property

        #region Type
        #endregion Type

        #region Constant
        #endregion Constant

        #region Field
        /// <summary> stores arguments by order of appeared plain args. </summary>
        private List<string> plainArgs = new List<string>();
        /// <summary> if the arg does not exist, the value will be null. </summary>
        private Dictionary<string, string> mapArgs = new Dictionary<string, string>();
        /// <summary> if there is a certain switch, the related item exists in switchArgs. </summary>
        private HashSet<string> switchArgs = new HashSet<string>();
        #endregion Field
    }
}

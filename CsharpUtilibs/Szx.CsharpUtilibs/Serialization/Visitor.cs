//#define DEBUG_VISITOR


using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;


namespace Szx.CsharpUtilibs.Serialization
{
    using Szx.CsharpUtilibs.Collections;


    /// <summary> determine what kinds of objects will be visited. </summary>
    public enum VisitPolicy
    {
        Public = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static,
        Instance = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
        All = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static
    }

    /// <summary> define actions on phases in traversing. </summary>
    /// <remarks> 
    /// [Attention] "event" in the following description means an event     <para />
    ///             pair consist of (OnEnter* and OnLeave*) or OnVisit*.    <para />
    ///                                                                     <para />
    /// if an event of On*Node is reached, it may come after any OnEnter*   <para />
    /// and is followed by corresponding OnLeave*.                          <para />
    ///                                                                     <para />
    /// if an event in "events on objects" is reached, it always comes      <para />
    /// after an OnEnterNode() and is followed by an OnLeaveNode().         <para />
    ///                                                                     <para />
    /// if an event in "events on collections" is reached, it always comes  <para />
    /// after an OnEnterNonLeaf() and is followed by an OnLeaveNonLeaf().   <para />
    /// </remarks>
    public interface IVisitor
    {
        void OnEnterTraverse(object obj = null);
        void OnLeaveTraverse(object obj = null);

        void OnEnterNode(object obj = null, FieldInfo fieldInfo = null);
        void OnLeaveNode(object obj = null, FieldInfo fieldInfo = null);

        #region events on objects
        void OnVisitLeaf(object obj = null, FieldInfo fieldInfo = null);

        void OnEnterNonLeaf(object obj = null, FieldInfo fieldInfo = null);
        void OnLeaveNonLeaf(object obj = null, FieldInfo fieldInfo = null);
        #endregion

        #region events on Collections
        void OnEnterArray(Array array = null);
        void OnLeaveArray(Array array = null);

        void OnEnterBitArray(BitArray bitArray = null);
        void OnLeaveBitArray(BitArray bitArray = null);

        void OnEnterQueueOrStack(dynamic queueOrStack = null);
        void OnLeaveQueueOrStack(dynamic queueOrStack = null);

        void OnEnterDictionary(IDictionary dictionary = null);
        void OnLeaveDictionary(IDictionary dictionary = null);

        void OnEnterList(IList list = null);
        void OnLeaveList(IList list = null);

        void OnEnterCollection(ICollection<object> collection = null);
        void OnLeaveCollection(ICollection<object> collection = null);
        #endregion

        VisitPolicy Policy { get; set; }
    }

    public class TextWriterVisitor : IVisitor
    {
        #region Constructor
        public TextWriterVisitor(TextWriter textWriter, VisitPolicy policy = VisitPolicy.All) {
            this.textWriter = textWriter;
            this.policy = policy;
        }
        #endregion

        #region Method
        private void Display() {
#if DEBUG_VISITOR
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write(output.ToString());
#endif
        }

        #region writters
        protected void Write(object msg) {
            output.Append(msg ?? NullString);
            Display();
        }
        protected void WriteLine() {
            Write(textWriter.NewLine + Indent);
        }
        protected void WriteInNewLine(object msg) {
            WriteLine(); Write(msg);
        }
        protected void WriteFieldNameInNewLine(FieldInfo fieldInfo) {
            WriteInNewLine(EnterNamePrompt + fieldInfo.GetFriendlyName()
                + LeaveNamePrompt + NameValueDelimiter);
        }
        protected void WriteValue(object obj) {
            Write(((obj is string) ? (EnterValuePrompt + obj + LeaveValuePrompt) : (obj)));
        }
        #endregion

        public void OnEnterTraverse(object obj = null) {
            Write(EnterClassPrompt);
        }

        public void OnLeaveTraverse(object obj = null) {
            output.Length -= ClassElementDelimiter.Length;
#if DEBUG_VISITOR
            Display();
            textWriter.WriteLine();
#else
            textWriter.WriteLine(output.ToString());
#endif
            textWriter.WriteLine(LeaveClassPrompt);
        }

        public void OnEnterNode(object obj = null, FieldInfo fieldInfo = null) {
            indent.Append(IndentDelta);
        }

        public void OnLeaveNode(object obj = null, FieldInfo fieldInfo = null) {
            Write(ElementDelimiter[(int)CurrentState]);
            indent.Length -= IndentDelta.Length;
        }

        public void OnVisitLeaf(object obj = null, FieldInfo fieldInfo = null) {
            if (fieldInfo != null) {
                WriteFieldNameInNewLine(fieldInfo);
            }
            WriteValue((obj is bool) ? obj.ToString().ToLower() : obj);
        }

        /// <summary>
        /// since it is agnostic after entering it, the top of the state stack 
        /// can be changed by other event.
        /// </summary>
        public void OnEnterNonLeaf(object obj = null, FieldInfo fieldInfo = null) {
            if (CurrentState == TraverseState.InClass) {
                WriteFieldNameInNewLine(fieldInfo);
            }
            Write(EnterClassPrompt);
            states.Push(TraverseState.InClass);
        }

        public void OnLeaveNonLeaf(object obj = null, FieldInfo fieldInfo = null) {
            if (CurrentState == TraverseState.InClass) {
                output.Length -= ClassElementDelimiter.Length;
                WriteInNewLine(LeaveClassPrompt);
            }
            states.Pop();
        }

        public void OnEnterArray(Array array = null) {
            OnEnterList();
        }

        public void OnLeaveArray(Array array = null) {
            OnLeaveList((array.Length == 0), array.GetType().GetElementType().IsPrintable());
        }

        public void OnEnterDictionary(IDictionary dictionary = null) {
            output.Length -= EnterClassPrompt.Length;
            Write(EnterDictionaryPrompt);
            // the last top of the stack must be InClass from OnEnterNonLeaf()
            states.Pop();   // reset the state to be InDictionary
            states.Push(TraverseState.InDictionary);
        }

        public void OnLeaveDictionary(IDictionary dictionary = null) {
            // let OnLeaveNonLeaf() pop the state
            output.Length -= DictionaryElementDelimiter.Length;
            WriteInNewLine(LeaveDictionaryPrompt);
        }

        public void OnEnterList(IList list = null) {
            output.Length -= EnterClassPrompt.Length;
            Write(EnterListPrompt);
            // the last top of the stack must be InClass from OnEnterNonLeaf()
            states.Pop();   // reset the state to be InList
            states.Push(TraverseState.InList);
        }

        public void OnLeaveList(IList list = null) {
            bool isEmpty = (list.Count == 0);
            OnLeaveList(isEmpty, isEmpty || list[0].GetType().IsPrintable());
        }

        /// <summary> 
        /// agent for list like collections whose type is incompatible with
        /// IList. As the list object is not in use currently.
        /// </summary>
        /// <param name="isEmpty"> if the list is empty. </param>
        /// <param name="isElementPrimitive"> 
        /// if (isEmpty == true), isElementPrimitive can be any value. 
        /// </param>
        private void OnLeaveList(bool isEmpty, bool isElementPrimitive) {
            // let OnLeaveNonLeaf() pop the state
            if (!isEmpty) {
                output.Length -= ListElementDelimiter.Length;
                if (!isElementPrimitive) { WriteLine(); }
            }
            Write(LeaveListPrompt);
        }

        public void OnEnterCollection(ICollection<object> collection = null) {
            OnEnterList();
        }

        public void OnLeaveCollection(ICollection<object> collection = null) {
            OnLeaveList((collection.Count == 0), collection.GetType().GetGenericArguments()[0].IsPrintable());
        }

        public void OnEnterBitArray(BitArray bitArray = null) {
            OnEnterList();
        }

        public void OnLeaveBitArray(BitArray bitArray = null) {
            OnLeaveList((bitArray.Count == 0), true);
        }

        public void OnEnterQueueOrStack(dynamic queueOrStack = null) {
            //OnEnterList();
        }

        // UNDONE[5] : same as list?
        public void OnLeaveQueueOrStack(dynamic queueOrStack = null) {
            
        }
        #endregion

        #region Property
        public VisitPolicy Policy {
            get { return policy; }
            set { policy = value; }
        }

        public TraverseState CurrentState {
            get { return states.Peek(); }
        }

        protected string Indent {
            get { return indent.ToString(); }
        }
        #endregion

        #region Type
        #endregion

        #region Constant
        public enum TraverseState
        {
            InClass, InList, InDictionary
        }

        protected const string NullString = "null";

        protected const string IndentDelta = "  ";

        protected const string EnterNamePrompt = "\"";
        protected const string LeaveNamePrompt = "\"";
        protected const string NameValueDelimiter = " : ";
        protected const string EnterValuePrompt = "\""; // also called leaf
        protected const string LeaveValuePrompt = "\""; // also called leaf

        // EnterClassPrompt must have spaces whose number is the 
        // same as LeaveClassPrompt.Length in case empty class
        protected const string EnterClassPrompt = "{ ";
        protected const string LeaveClassPrompt = "}";
        protected const string ClassElementDelimiter = ",";

        protected const string EnterListPrompt = "[";
        protected const string LeaveListPrompt = "]";
        protected const string ListElementDelimiter = ", ";

        protected const string EnterDictionaryPrompt = "[";
        protected const string LeaveDictionaryPrompt = "]";
        protected const string DictionaryElementDelimiter = ",";
        protected const string KeyValueDelimiter = " : ";

        protected readonly string[] EnterPrompt = new string[] {
            EnterClassPrompt, EnterListPrompt, EnterDictionaryPrompt
        };
        protected readonly string[] LeavePrompt = new string[] {
            LeaveClassPrompt, LeaveListPrompt, LeaveDictionaryPrompt
        };
        protected readonly string[] ElementDelimiter = new string[] {
            ClassElementDelimiter, ListElementDelimiter, DictionaryElementDelimiter
        };
        #endregion

        #region Field
        protected Stack<TraverseState> states = new Stack<TraverseState>(
            new TraverseState[] { TraverseState.InClass });

        private StringBuilder output = new StringBuilder();
        private StringBuilder indent = new StringBuilder();

        private TextWriter textWriter;

        private VisitPolicy policy;
        #endregion
    }

    //public class BinaryWriterVisitor : IVisitor
    //{
    //    #region Constructor
    //    public BinaryWriterVisitor(Stream s) {
    //        writer = new BinaryWriter(s);
    //    }
    //    #endregion

    //    #region Method
    //    #endregion

    //    #region Property
    //    #endregion

    //    #region Type
    //    #endregion

    //    #region Constant
    //    #endregion

    //    #region Field
    //    private BinaryWriter writer;
    //    #endregion
    //}
}

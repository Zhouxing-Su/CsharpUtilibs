using System;


namespace IDeal.Szx.CsharpUtilibs.Random {
    public class Select {
        /// <summary>
        /// sometimes the first element is pre-selected with the possibility 
        /// of 1, so you can pass 1 into the constructor in this condition to 
        /// leave out a isSelected() call.
        /// </summary>
        public Select(int startCount = 1) {
            count = startCount;
        }

        /// <summary> 
        /// start a new selection on another N elements.
        /// sometimes the first element is pre-selected with the possibility of 1, 
        /// so you can pass 1 in this condition to leave out a isSelected() call.
        /// </summary>
        public void reset(int startCount = 1) {
            count = startCount;
        }

        /// <summary>
        /// call this for each of the N elements (N times in total) to judge 
        /// whether each of them is selected. 
        /// only the last returned "true" means that element is selected finally.
        /// </summary>
        public bool isSelected(int randNum) {
            return ((randNum % (++count)) == 0);
        }

        public bool isMinimal(dynamic target, dynamic min, int randNum) {
            if (target < min) {
                reset();
                return true;
            } else if (target == min) {
                return isSelected(randNum);
            } else {
                return false;
            }
        }
        public bool isMinimal(int target, int min, int randNum) {
            if (target < min) {
                reset();
                return true;
            } else if (target == min) {
                return isSelected(randNum);
            } else {
                return false;
            }
        }
        public bool isMinimal(long target, long min, int randNum) {
            if (target < min) {
                reset();
                return true;
            } else if (target == min) {
                return isSelected(randNum);
            } else {
                return false;
            }
        }
        public bool isMinimal(double target, double min, int randNum) {
            if (target < min) {
                reset();
                return true;
            } else if (target == min) {
                return isSelected(randNum);
            } else {
                return false;
            }
        }

        private int count;
    }
}

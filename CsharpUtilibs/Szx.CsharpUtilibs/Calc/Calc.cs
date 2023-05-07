using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeal.Szx.CsharpUtilibs.Calc {
    public static class Calc {
        public static int Count(params bool[] binaries) { return binaries.Count(t => t); }

        public static bool UpdateMin(ref int minValue, int newValue) {
            if (newValue < minValue) { minValue = newValue; return true; }
            return false;
        }
        public static bool UpdateMax(ref int maxValue, int newValue) {
            if (newValue > maxValue) { maxValue = newValue; return true; }
            return false;
        }
        public static bool UpdateMin(ref double minValue, double newValue) {
            if (newValue < minValue) { minValue = newValue; return true; }
            return false;
        }
        public static bool UpdateMax(ref double maxValue, double newValue) {
            if (newValue > maxValue) { maxValue = newValue; return true; }
            return false;
        }
        public static bool UpdateMin(ref string minValue, string newValue) {
            if (newValue.CompareTo(minValue) < 0) { minValue = newValue; return true; }
            return false;
        }
        public static bool UpdateMax(ref string maxValue, string newValue) {
            if (newValue.CompareTo(maxValue) > 0) { maxValue = newValue; return true; }
            return false;
        }

        public static int LowerBound(int[] arr, int value) {
            int i = Array.BinarySearch(arr, value);
            return (i < 0) ? ~i : i;
        }

        public static double PowerSum(double a, double b) {
            return a * a + b * b;
        }


        // "forward" means `(src0 <= dst0) && (src1 <= dst1)`.
        public static class RectilinearForward {
            public static bool between(int value, int lb, int ub) { return (lb <= value) && (value <= ub); }

            public static bool within(int[] pos, int[] corner0, int[] corner1) {
                for (int i = 0; i < pos.Length; ++i) {
                    if (!between(pos[i], corner0[i], corner1[i])) { return false; }
                }
                return true;
            }
            public static bool within(int[] pos, int[] box) {
                return within(pos, new int[2] { box[0], box[1] }, new int[2] { box[2], box[3] });
            }

            public static bool segmentsInterfering(int src0, int dst0, int src1, int dst1) { return (src0 <= dst1) && (src1 <= dst0); }

            /// <summary>
            /// check if two rectilinear forward segments are intersected or overlapped.
            /// </summary>
            public static bool segmentsInterfering(int[] src0, int[] dst0, int[] src1, int[] dst1) {
                int dx0 = dst0[0] - src0[0];
                int dx1 = dst1[0] - src1[0];
                if ((dx0 == 0) != (dx1 == 0)) { // orthogonal.
                    if (dx0 == 0) { // segment 0 is vertical and segment 1 is horizontal.
                        return between(src0[0], src1[0], dst1[0]) && between(src1[1], src0[1], dst0[1]);
                    } else { // segment 0 is horizontal and segment 1 is vertical.
                        return between(src1[0], src0[0], dst0[0]) && between(src0[1], src1[1], dst1[1]);
                    }
                } else { // parallel.
                    if (dx0 == 0) { // both are vertical.
                        return (src0[0] == src1[0]) && segmentsInterfering(src0[1], dst0[1], src1[1], dst1[1]);
                    } else { // both are horizontal.
                        return (src0[1] == src1[1]) && segmentsInterfering(src0[0], dst0[0], src1[0], dst1[0]);
                    }
                }
            }
            public static bool segmentsInterfering(int[][] segment0, int[][] segment1) {
                return segmentsInterfering(segment0[0], segment0[1], segment1[0], segment1[1]);
            }

            /// <summary>
            /// check if a rectilinear segments and a box (hollow rectangle) is intersected or overlapped.
            /// </summary>
            public static bool segmentBoxInterfering(int[][] segment, int[] box) {
                int dx0 = segment[1][0] - segment[0][0];
                if (dx0 == 0) { // vertical.
                    return between(segment[0][0], box[0], box[2]) && segmentsInterfering(segment[0][1], segment[1][1], box[1], box[3]);
                } else { // horizontal.
                    return between(segment[0][1], box[1], box[3]) && segmentsInterfering(segment[0][0], segment[1][0], box[0], box[2]);
                }
            }

            /// <summary>
            /// check if a rectilinear segments and a solid rectangle is intersected or overlapped.
            /// </summary>
            public static bool segmentRectInterfering(int[][] segment, int[] rect) {
                return segmentBoxInterfering(segment, rect)
                    || (within(segment[0], rect) && within(segment[1], rect));
            }
        }

        public static class Rectilinear {
            public static bool between(int value, int bound0, int bound1) {
                //return ((value - bound0) * (value - bound1)) <= 0; // may overflow.
                return ((value == bound0) || (value == bound1)) || ((value < bound0) != (value < bound1));
                //return (value <= bound0) ? (value >= bound1) : (value <= bound1);
            }

            public static bool within(int[] pos, int[] corner0, int[] corner1) {
                for (int i = 0; i < pos.Length; ++i) {
                    if (!between(pos[i], corner0[i], corner1[i])) { return false; }
                }
                return true;
            }
            public static bool within(int[] pos, int[] box) {
                return within(pos, new int[2] { box[0], box[1] }, new int[2] { box[2], box[3] });
            }

            public static bool segmentsInterfering(int src0, int dst0, int src1, int dst1) {
                int r = Count(src0 <= src1, src0 < dst1, dst0 < src1, dst0 <= dst1);
                return (0 < r) && (r < 4);
            }

            /// <summary>
            /// check if two rectilinear segments are intersected or overlapped.
            /// </summary>
            public static bool segmentsInterfering(int[] src0, int[] dst0, int[] src1, int[] dst1) {
                int dx0 = dst0[0] - src0[0];
                int dx1 = dst1[0] - src1[0];
                if ((dx0 == 0) != (dx1 == 0)) { // orthogonal.
                    if (dx0 == 0) { // segment 0 is vertical and segment 1 is horizontal.
                        return between(src0[0], src1[0], dst1[0]) && between(src1[1], src0[1], dst0[1]);
                    } else { // segment 0 is horizontal and segment 1 is vertical.
                        return between(src1[0], src0[0], dst0[0]) && between(src0[1], src1[1], dst1[1]);
                    }
                } else { // parallel.
                    if (dx0 == 0) { // both are vertical.
                        if (src0[0] != src1[0]) { return false; }
                        return segmentsInterfering(src0[1], dst0[1], src1[1], dst1[1]);
                    } else { // both are horizontal.
                        if (src0[1] != src1[1]) { return false; }
                        return segmentsInterfering(src0[0], dst0[0], src1[0], dst1[0]);
                    }
                }
            }
            public static bool segmentsInterfering(int[][] segment0, int[][] segment1) {
                return segmentsInterfering(segment0[0], segment0[1], segment1[0], segment1[1]);
            }

            /// <summary>
            /// check if a rectilinear segments and a box (hollow rectangle) is intersected or overlapped.
            /// </summary>
            public static bool segmentBoxInterfering(int[][] segment, int[] box) {
                int dx0 = segment[0][1] - segment[0][0];
                int[] n00 = new int[2] { box[0], box[1] };
                int[] n01 = new int[2] { box[0], box[3] };
                int[] n10 = new int[2] { box[2], box[1] };
                int[] n11 = new int[2] { box[2], box[3] };
                if (dx0 == 0) { // vertical.
                    return segmentsInterfering(segment, new int[2][] { n00, n10 })
                        || segmentsInterfering(segment, new int[2][] { n01, n11 });
                } else { // horizontal.
                    return segmentsInterfering(segment, new int[2][] { n00, n01 })
                        || segmentsInterfering(segment, new int[2][] { n10, n11 });
                }
            }

            /// <summary>
            /// check if a rectilinear segments and a solid rectangle is intersected or overlapped.
            /// </summary>
            public static bool segmentRectInterfering(int[][] segment, int[] rect) {
                return segmentBoxInterfering(segment, rect)
                    || (within(segment[0], rect) && within(segment[1], rect));
            }
        }


        public class OppositeComparer<T> : IComparer<T> {
            public static OppositeComparer<T> Default { get { return oppositeComparer; } }

            public int Compare(T x, T y) { return -Comparer<T>.Default.Compare(x, y); }

            protected static OppositeComparer<T> oppositeComparer = new OppositeComparer<T>();
        }

        public static void Swap<T>(ref T l, ref T r) {
            T tmp = l;
            l = r;
            r = tmp;
        }
    }
}

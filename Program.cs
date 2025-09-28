using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vectors
{
    public class Vector : IList<double>
    {
        private IList<double> _vector;
        private int _size;

        public static Vector i = new Vector(new double[] { 1, 0, 0 });
        public static Vector j = new Vector(new double[] { 0, 1, 0 });
        public static Vector k = new Vector(new double[] { 0, 0, 1 });

        public int Size => _size;
        public int Count => _size;
        public bool IsReadOnly => false;

        public Vector(int size)
        {
            _vector = new double[size];
            _size = size;
        }
        public static Vector Create<T>(IList<T> vector)
        {
            double[] values = vector.Select(x => Convert.ToDouble(x)).ToArray();
            return new Vector(values);
        }
        public static Vector Create<T>(IList<T> vector, int size)
        {
            double[] values = vector.Select(x => Convert.ToDouble(x)).ToArray();
            return new Vector(values, size);
        }

        public Vector(double[] vector)
        {
            _vector = vector;
            _size = vector.Length;
        }
        public Vector(double[] vector, int size)
        {
            if (vector.Length != size)
            {
                throw new Exception();
            }
            _vector = vector;
            _size = size;
        }

        public double this[int index]
        {
            get => _vector[index];
            set => _vector[index] = value;
        }

        public static implicit operator double[](Vector v) => (double[])v._vector;

        public int IndexOf(double item) => throw new NotSupportedException("Vector class does not support this method.");
        public void Insert(int index, double item) => throw new NotSupportedException("Vector class does not support this method.");
        public void RemoveAt(int index) => throw new NotSupportedException("Vector class does not support this method.");
        public void Add(double item) => throw new NotSupportedException("Vector class does not support this method.");
        public void Clear() => _vector = null;
        public bool Contains(double item) => throw new NotSupportedException("Vector class does not support this method.");
        public void CopyTo(double[] array, int arrayIndex) => throw new NotSupportedException("Vector class does not support this method.");
        public bool Remove(double item) => throw new NotSupportedException("Vector class does not support this method.");


        public double Length()
        {
            double length = 0;
            for (int i = 0; i < Size; i++)
            {
                length += this[i] * this[i];
            }
            return Math.Sqrt(length);
        }

        public Vector Unit() => this / Length();
        public bool IsUnit() => Length() == 1;

        public Vector ProjectOn(Vector v)
        {
            if (Size != v.Size)
            {
                throw new Exception();
            }
            double scalar = this * v;
            return this * (scalar / Math.Pow(this.Length(), 2));
        }
        public Vector ProjectFrom(Vector v)
        {
            return v.ProjectOn(this);
        }
        public Vector GramShmidt(Vector v)
        {
            Vector proj = ProjectOn(v);
            return (v - proj).Unit();
        }

        public double Angle(Vector v)
        {
            double scalar = this * v;
            double len = this.Length() * v.Length();

            if (len == 0)
            {
                throw new DivideByZeroException("Dot vectors do not have an angle.");
            }

            double cos = scalar / len;
            return Math.Acos(cos);
        }
        public Vector Cross(Vector v)
        {
            if (this.Size != 3 || v.Size != 3)
            {
                throw new Exception("Cannot compute cross product for non-3D vectors.");
            }

            double deti = this[1] * v[2] - this[2] * v[1];
            double detj = this[0] * v[2] - this[2] * v[0];
            double detk = this[0] * v[1] - this[1] * v[0];

            Vector cross = Vector.i * deti + Vector.j * detj + Vector.k * detk;
            return cross;
        }

        public static Vector operator -(Vector negative) => negative * -1;
        public static Vector operator +(Vector first, Vector second)
        {
            if (first.Size != second.Size) throw new Exception();

            Vector sum = new Vector(first.Size);
            for (int i = 0; i < first.Size; i++)
            {
                sum[i] = first[i] + second[i];
            }
            return sum;
        }
        public static Vector operator -(Vector first, Vector second)
        {
            if (first.Size != second.Size) throw new Exception();

            Vector diff = new Vector(first.Size);
            for (int i = 0; i < first.Size; i++)
            {
                diff[i] = first[i] - second[i];
            }
            return diff;
        }

        public static double operator *(Vector first, Vector second)
        {
            if (first.Size != second.Size) throw new Exception();

            double scalar = 0;
            for (int i = 0; i < first.Size; i++)
            {
                scalar += first[i] * second[i];
            }
            return scalar;
        }
        public static Vector operator *(Vector first, double second)
        {
            Vector mult = new Vector(first.Size);
            for (int i = 0; i < first.Size; i++)
            {
                mult[i] = first[i] * second;
            }
            return mult;
        }
        public static Vector operator *(double first, Vector second) => second * first;
        public static Vector operator /(Vector first, double second)
        {
            if (second == 0) throw new DivideByZeroException();

            Vector div = new Vector(first.Size);
            for (int i = 0; i < first.Size; i++)
            {
                div[i] = first[i] / second;
            }
            return div;
        }

        public static bool operator ==(Vector first, Vector second)
        {
            if (first.Size != second.Size) return false;

            for (int i = 0; i < first.Size; i++)
            {
                if (first[i] != second[i]) return false;
            }
            return true;
        }
        public static bool operator !=(Vector first, Vector second) => !(first == second);
        public override bool Equals(object second) => this == (Vector)second;
        public override int GetHashCode() => base.GetHashCode();

        public override string ToString()
        {
            string vector = "[ ";
            foreach (double i in _vector)
            {
                vector += $"{i} ";
            }
            vector += "]";
            return vector;
        }
        public void print()
        {
            Console.WriteLine(this);
        }

        public IEnumerator<double> GetEnumerator() => _vector.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _vector.GetEnumerator();

    }

    class Program
    {
        static void Main(string[] args)
        {
            //var i = new List<double>();
            var f = Vector.Create(new double[] { 1, 2, 3 });
            Console.WriteLine(f);
            Vector w = Vector.Create(new int[] { 1, 3, 4 });
            Console.WriteLine(w);
            Vector v = Vector.Create(new List<double> { 0, 9, 0 });
            Console.WriteLine(v);

            // Example future usage:
            // Matrix<int[], int> a = new Matrix<int[], int>(3, 4);
            // Matrix<Vector, double> a = new Matrix<Vector, double>(3, 4);
            // a.Print();
        }
    }
}

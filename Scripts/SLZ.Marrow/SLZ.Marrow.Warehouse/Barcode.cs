using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace SLZ.Marrow.Warehouse
{
    [Serializable]
    public class Barcode : IEquatable<Barcode>, ISerializationCallbackReceiver
    {
        public static bool useShortCode = false;
        public static bool useCachedHashCode = false;
        [SerializeField]
        private string _id = EMPTY;
        public string ID
        {
            get => _id;
            private set
            {
                _id = value;
                if (useShortCode)
                {
                    shortCodeGenerated = false;
                    GenerateShortCode();
                    cachedHashCode = -1;
                }
            }
        }

        private bool shortCodeGenerated = false;
        private uint _shortCode = 0;
        public uint ShortCode
        {
            get
            {
                GenerateShortCode();
                return _shortCode;
            }

            private set => _shortCode = value;
        }

        public static string separator = ".";
        public static readonly string EMPTY = BuildBarcode("null", "empty", "barcode");
        private static readonly string EMPTY_OLD = "00000000-0000-0000-0000-000000000000";
        public static readonly int MAX_SIZE = 120;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        static readonly Unity.Profiling.ProfilerMarker getHashcodeProfileMarker = new Unity.Profiling.ProfilerMarker("Barcode.GetHashCode");
#endif
        private int cachedHashCode = -1;
        public static Barcode EmptyBarcode()
        {
            return new Barcode(EMPTY);
        }

        public Barcode()
        {
        }

        public Barcode(Barcode other)
        {
            if (other != null)
            {
                ID = other.ID;
            }
        }

        public Barcode(string newId)
        {
            ID = newId;
        }

        public static string BuildBarcode(params string[] parts)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (var part in parts)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(separator);
                }

                sb.Append(MarrowSDK.SanitizeID(part));
            }

            return sb.ToString();
        }

        public static bool IsValidSize(Barcode barcode)
        {
            if (barcode == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(barcode.ID))
            {
                return true;
            }

            return barcode.ID.Length <= MAX_SIZE;
        }

        public static bool IsValidSize(string barcodeString)
        {
            if (string.IsNullOrEmpty(barcodeString))
            {
                return true;
            }

            return barcodeString.Length <= MAX_SIZE;
        }

        public static bool IsValid(Barcode barcode)
        {
            bool valid = true;
            if (barcode == null)
            {
                valid = false;
            }
            else if (string.IsNullOrEmpty(barcode.ID))
            {
                valid = false;
            }
            else if (barcode.ID == EMPTY || barcode.ID == EMPTY_OLD)
            {
                valid = false;
            }

            return valid;
        }

        public static bool IsValidString(string barcodeString)
        {
            bool valid = true;
            if (string.IsNullOrEmpty(barcodeString))
            {
                valid = false;
            }
            else if (barcodeString == EMPTY || barcodeString == EMPTY_OLD)
            {
                valid = false;
            }

            return valid;
        }

        public void GenerateID(params string[] parts)
        {
            GenerateID(false, parts);
        }

        public void GenerateID(bool forceGeneration = false, params string[] parts)
        {
            if (!this.IsValid() || forceGeneration)
            {
                ID = BuildBarcode(parts);
            }
        }

        private static Dictionary<string, uint> shortCodeDictionary = new Dictionary<string, uint>();
        private void GenerateShortCode()
        {
            if (!shortCodeGenerated)
            {
                if (shortCodeDictionary.TryGetValue(ID, out uint cachedShortCode))
                {
                    _shortCode = cachedShortCode;
                }
                else
                {
                    _shortCode = shortCodeDictionary[ID] = (uint)shortCodeDictionary.Count;
                }

                shortCodeGenerated = true;
            }
        }

        public override string ToString() => ID;
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj is Barcode barcode)
                return Equals(barcode);
            if (obj is string objString)
                return Equals(new Barcode(objString));
            return false;
        }

        public bool Equals(Barcode other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

            if (useShortCode)
                return this.ShortCode.Equals(other.ShortCode);
            else
                return this.ID.Equals(other.ID);
        }

        public static bool operator ==(Barcode barcode, Barcode otherBarcode)
        {
            if (barcode is null)
            {
                if (otherBarcode is null)
                {
                    return true;
                }

                return false;
            }

            return barcode.Equals(otherBarcode);
        }

        public static bool operator !=(Barcode barcode, Barcode otherBarcode)
        {
            return !(barcode == otherBarcode);
        }

        public override int GetHashCode()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            getHashcodeProfileMarker.Begin();
#endif
            int hashCode;
            if (useCachedHashCode)
            {
                if (cachedHashCode == -1)
                {
                    if (useShortCode)
                        cachedHashCode = hashCode = ShortCode.GetHashCode();
                    else
                        cachedHashCode = hashCode = ID.GetHashCode();
                }
                else
                {
                    hashCode = cachedHashCode;
                }
            }
            else
            {
                if (useShortCode)
                    hashCode = ShortCode.GetHashCode();
                else
                    hashCode = ID.GetHashCode();
            }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            getHashcodeProfileMarker.End();
#endif
            return hashCode;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            if (!this.IsValid())
            {
                this.ID = EMPTY;
            }

            if (useShortCode)
            {
                shortCodeGenerated = false;
                GenerateShortCode();
            }

            cachedHashCode = -1;
#endif
        }

        public static bool operator true(Barcode barcode) => IsValid(barcode);
        public static bool operator false(Barcode barcode) => !IsValid(barcode);
        public static implicit operator bool(Barcode barcode) => IsValid(barcode);
    }

    public static class BarcodeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(this Barcode barcode) => Barcode.IsValid(barcode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidSize(this Barcode barcode) => Barcode.IsValidSize(barcode);
    }
}
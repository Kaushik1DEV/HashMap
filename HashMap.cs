using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure
{
    public class HashMap
    {
        private class HashNode
        {
            public string key;
            public int value;

            public HashNode(string key, int value)
            {
                this.key = key;
                this.value = value;
            }
        }

        private int size;
        private LinkedList<HashNode>[] buckets;

        public HashMap()
        {
            InitBuckets(4);
            size = 0;
        }

        private void InitBuckets(int N)
        {
            buckets = new LinkedList<HashNode>[N];
            for (int i = 0; i < N; i++)
            {
                buckets[i] = new LinkedList<HashNode>();
            }
        }

        public void Put(string key, int value)
        {
            int bucketIndex = HashFunction(key);
            int indexWithinTheBucket = GetIndexWithinTheBucket(key, bucketIndex);

            if (indexWithinTheBucket != -1)
            {
                IEnumerator<HashNode> enumerator = buckets[bucketIndex].GetEnumerator();
                HashNode hashNode = null;

                while (enumerator.MoveNext())
                {
                    if (key == enumerator.Current.key)
                    {
                        hashNode = enumerator.Current;
                        break;
                    }

                }
                hashNode.value = value;
            }
            else
            {
                HashNode hashNode = new HashNode(key, value);
                buckets[bucketIndex].AddLast(hashNode);
                size++;
            }

            double lambda = size * 1.0 / buckets.Length;

            if (lambda > 2.0)
            {
                ReHash();
            }

        }

        private void ReHash()
        {
            LinkedList<HashNode>[] oldHashMap = buckets;
            InitBuckets(oldHashMap.Length * 2);

            for (int i = 0; i < oldHashMap.Length; i++)
            {
                foreach (HashNode hashNode in oldHashMap[i])
                {
                    Put(hashNode.key, hashNode.value);
                }
            }

        }

        public bool ContainsKey(string key)
        {
            int bucketIndex = HashFunction(key);
            int indexWithinBucket = GetIndexWithinTheBucket(key, bucketIndex);

            if (indexWithinBucket != -1) return true;
            return false;
        }

        public int? Get(string key)
        {
            int bucketIndex = HashFunction(key);
            int indexWithinBucket = GetIndexWithinTheBucket(key, bucketIndex);
            HashNode hashNode = null;

            try
            {

                if (indexWithinBucket != -1 && buckets != null)
                {
                    IEnumerator<HashNode> enumerator = buckets[bucketIndex].GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (key == enumerator.Current.key)
                        {
                            hashNode = enumerator.Current;
                            break;
                        }
                    }
                }
            }catch(Exception e)
            {
                throw new Exception("HashMap is Empty", e);
            }

            return hashNode.value;
        }

        public int? Remove(string key)
        {
            int bucketIndex = HashFunction(key);
            int indexWithinBucket = GetIndexWithinTheBucket(key, bucketIndex);
            HashNode hashNode = null;

            if (indexWithinBucket != -1)
            {
                IEnumerator<HashNode> enumerator = buckets[bucketIndex].GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (key == enumerator.Current.key)
                    {
                        hashNode = enumerator.Current;
                        break;
                    }
                }

                buckets[bucketIndex].Remove(hashNode);
                return hashNode.value;
            }

            return null;
        }

        public int Size()
        {
            return size;
        }

        public List<string> KeySet()
        {
            List<string> keySet = new List<string>();
            try
            {
                if (buckets != null)
                {
                    for (int i = 0; i < buckets.Length; i++)
                    {
                        foreach (HashNode hashNode in buckets[i])
                        {
                            keySet.Add(hashNode.key);
                        }
                    }
                }
            }catch(Exception e)
            {
                throw new Exception("HashMap is empty", e);
            }

            return keySet;
        }

        private int HashFunction(string key)
        {
            int hashCode = key.GetHashCode();
            return Math.Abs(hashCode)%buckets.Length;
        }

        private int GetIndexWithinTheBucket(string key, int bucketIndex) 
        {
            int dataIndex = 0;
            try
            {
                if (buckets != null)
                {
                    foreach (HashNode hash in buckets[bucketIndex])
                    {
                        if (hash.key.Equals(key))
                        {
                            return dataIndex;
                        }

                        dataIndex++;
                    }
                }
            }catch(Exception e)
            {
                throw new Exception("HashMap is Empty",e);
            }
            return -1;
        }
    }
}

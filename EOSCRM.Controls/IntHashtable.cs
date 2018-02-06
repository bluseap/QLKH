using System;

namespace EOSCRM.Controls
{
    public class IntHashtable
    {
        private int count;
        private float loadFactor;
        private IntHashtableEntry[] table;
        private int threshold;

        public IntHashtable() : this(0x65, 0.75f)
        {
        }

        public IntHashtable(int initialCapacity) : this(initialCapacity, 0.75f)
        {
        }

        public IntHashtable(int initialCapacity, float loadFactor)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentException("illegal.capacity.1");
            }
            if (loadFactor <= 0f)
            {
                throw new ArgumentException("illegal.load.1");
            }
            this.loadFactor = loadFactor;
            this.table = new IntHashtableEntry[initialCapacity];
            this.threshold = (int) (initialCapacity * loadFactor);
        }

        public void Clear()
        {
            IntHashtableEntry[] table = this.table;
            int length = table.Length;
            while (--length >= 0)
            {
                table[length] = null;
            }
            this.count = 0;
        }

        public IntHashtable Clone()
        {
            IntHashtable hashtable = new IntHashtable {
                count = this.count,
                loadFactor = this.loadFactor,
                threshold = this.threshold,
                table = new IntHashtableEntry[this.table.Length]
            };
            int length = this.table.Length;
            while (length-- > 0)
            {
                hashtable.table[length] = (this.table[length] != null) ? this.table[length].Clone() : null;
            }
            return hashtable;
        }

        public bool Contains(int value)
        {
            IntHashtableEntry[] table = this.table;
            int length = table.Length;
            while (length-- > 0)
            {
                for (IntHashtableEntry entry = table[length]; entry != null; entry = entry.next)
                {
                    if (entry.value == value)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ContainsKey(int key)
        {
            IntHashtableEntry[] table = this.table;
            int num = key;
            int index = (num & 0x7fffffff) % table.Length;
            for (IntHashtableEntry entry = table[index]; entry != null; entry = entry.next)
            {
                if ((entry.hash == num) && (entry.key == key))
                {
                    return true;
                }
            }
            return false;
        }

        public IntHashtableIterator GetEntryIterator()
        {
            return new IntHashtableIterator(this.table);
        }

        public int[] GetKeys()
        {
            int[] numArray = new int[this.count];
            int num = 0;
            int length = this.table.Length;
            IntHashtableEntry next = null;
        Label_0019:
            if (next == null)
            {
                while ((length-- > 0) && ((next = this.table[length]) == null))
                {
                }
            }
            if (next != null)
            {
                IntHashtableEntry entry2 = next;
                next = entry2.next;
                numArray[num++] = entry2.key;
                goto Label_0019;
            }
            return numArray;
        }

        public bool IsEmpty()
        {
            return (this.count == 0);
        }

        protected void Rehash()
        {
            int length = this.table.Length;
            IntHashtableEntry[] table = this.table;
            int num2 = (length * 2) + 1;
            IntHashtableEntry[] entryArray2 = new IntHashtableEntry[num2];
            this.threshold = (int) (num2 * this.loadFactor);
            this.table = entryArray2;
            int index = length;
            while (index-- > 0)
            {
                IntHashtableEntry next = table[index];
                while (next != null)
                {
                    IntHashtableEntry entry2 = next;
                    next = next.next;
                    int num4 = (entry2.hash & 0x7fffffff) % num2;
                    entry2.next = entryArray2[num4];
                    entryArray2[num4] = entry2;
                }
            }
        }

        public int Remove(int key)
        {
            IntHashtableEntry[] table = this.table;
            int num = key;
            int index = (num & 0x7fffffff) % table.Length;
            IntHashtableEntry next = table[index];
            IntHashtableEntry entry2 = null;
            while (next != null)
            {
                if ((next.hash == num) && (next.key == key))
                {
                    if (entry2 != null)
                    {
                        entry2.next = next.next;
                    }
                    else
                    {
                        table[index] = next.next;
                    }
                    this.count--;
                    return next.value;
                }
                entry2 = next;
                next = next.next;
            }
            return 0;
        }

        public int[] ToOrderedKeys()
        {
            int[] keys = this.GetKeys();
            Array.Sort<int>(keys);
            return keys;
        }

        public int this[int key]
        {
            get
            {
                IntHashtableEntry[] table = this.table;
                int num = key;
                int index = (num & 0x7fffffff) % table.Length;
                for (IntHashtableEntry entry = table[index]; entry != null; entry = entry.next)
                {
                    if ((entry.hash == num) && (entry.key == key))
                    {
                        return entry.value;
                    }
                }
                return 0;
            }
            set
            {
                IntHashtableEntry[] table = this.table;
                int num = key;
                int index = (num & 0x7fffffff) % table.Length;
                for (IntHashtableEntry entry = table[index]; entry != null; entry = entry.next)
                {
                    if ((entry.hash == num) && (entry.key == key))
                    {
                        entry.value = value;
                        return;
                    }
                }
                if (this.count >= this.threshold)
                {
                    this.Rehash();
                    this[key] = value;
                }
                else
                {
                    table[index] = new IntHashtableEntry { hash = num, key = key, value = value, next = table[index] };
                    this.count++;
                }
            }
        }

        public int Size
        {
            get
            {
                return this.count;
            }
        }

        public class IntHashtableEntry
        {
            internal int hash;
            internal int key;
            internal IntHashtable.IntHashtableEntry next;
            internal int value;

            protected internal IntHashtable.IntHashtableEntry Clone()
            {
                return new IntHashtable.IntHashtableEntry { hash = this.hash, key = this.key, value = this.value, next = (this.next != null) ? this.next.Clone() : null };
            }

            public int Key
            {
                get
                {
                    return this.key;
                }
            }

            public int Value
            {
                get
                {
                    return this.value;
                }
            }
        }

        public class IntHashtableIterator
        {
            private IntHashtable.IntHashtableEntry entry;
            private int index;
            private IntHashtable.IntHashtableEntry[] table;

            internal IntHashtableIterator(IntHashtable.IntHashtableEntry[] table)
            {
                this.table = table;
                this.index = table.Length;
            }

            public bool HasNext()
            {
                if (this.entry == null)
                {
                    while (this.index-- > 0)
                    {
                        this.entry = this.table[this.index];
                        if (this.entry != null)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return true;
            }

            public IntHashtable.IntHashtableEntry Next()
            {
                if (this.entry == null)
                {
                    while ((this.index-- > 0) && ((this.entry = this.table[this.index]) == null))
                    {
                    }
                }
                if (this.entry == null)
                {
                    throw new InvalidOperationException("inthashtableiterator");
                }
                IntHashtable.IntHashtableEntry entry = this.entry;
                this.entry = entry.next;
                return entry;
            }
        }
    }
}


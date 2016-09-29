using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace Common.Runtime.Serialization
{
    using Attributes;
    using Transformation;

    internal class ConvertersHash<T>
    {
        private DataTable _hash;

        internal Serializer<T> this[Type type
                                     , PropertyInfo property
                                     , ISerializableProperty attribute
                                     //, string format
                                     //, Transformator transformator
                                     ]
        {
            get
            {
                DataRow row =  _hash.Rows.Find(GetHash(type
                                                    , property
                                                    , attribute
                                                    //, format
                                                    //, transformator
                                                    ));
                if (row != null)
                    return (Serializer<T>) row[4];

                return null;
            }
        }

        internal ConvertersHash()
        {

            _hash = new DataTable();

            _hash.Columns.AddRange(new DataColumn[] {
                new DataColumn("type", typeof(Guid)),
                new DataColumn("declaringType", typeof(Guid)),
                new DataColumn("property", typeof(string)),
                new DataColumn("attribute", typeof(string)),
                //new DataColumn("", typeof(Guid)),
                //new DataColumn("", typeof(Guid)),
                //new DataColumn("", typeof(Guid)),
                //new DataColumn("", typeof(Guid)),
                //new DataColumn("", typeof(Guid)),
                //new DataColumn("", typeof(Guid)),
                //new DataColumn("", typeof(Guid)),
                new DataColumn("serializer", typeof(Serializer<T>))
            });

            _hash.Constraints.Add(new UniqueConstraint(new DataColumn[] {
                _hash.Columns[0],
                _hash.Columns[1],
                _hash.Columns[2],
                _hash.Columns[3]
                //_hash.Columns[4],
                //_hash.Columns[5],
                //_hash.Columns[6],
                //_hash.Columns[7],
                //_hash.Columns[8],
                //_hash.Columns[9]
            }, true));

        }

        private object[] GetHash(Type type
                               , PropertyInfo property
                               , ISerializableProperty attribute
                               //, string format
                               //, Transformator transformator
            )
        {
            object[] hash = new object[] {
                type.GUID
              , property.DeclaringType.GUID
              , property.Name
              , attribute.Name
            };

            return hash;
        }

        private object[] GetHash(Serializer<T> serializer)
        {
            return GetHash(serializer.Type
                         , serializer.Property
                         , serializer.Attribute
                         //, null
                         //, serializer.Transformator
                         );
        }

        internal void Add(Serializer<T> serializer)
        {
            object[] hash = GetHash(serializer);

            _hash.Rows.Add(new object[] {
                hash[0],
                hash[1],
                hash[2],
                hash[3],
                //hash[4],
                //hash[5],
                //hash[6],
                //hash[7],
                //hash[8],
                //hash[9],
                serializer
            });
        }

    }
}

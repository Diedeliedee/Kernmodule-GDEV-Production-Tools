using System.Collections.Generic;

namespace BodyPartSwap
{
    public class Catalogue
    {
        private Dictionary<GlobalInfo.Types, PartQueue> m_dictionary = new();

        public Dictionary<GlobalInfo.Types, PartQueue> dictionary => m_dictionary;

        public Catalogue()
        {
            //  Create a dictionary entry for each body part type.
            for (int i = 0; i < GlobalInfo.typeAmount; i++)
            {
                var type = (GlobalInfo.Types)i;

                m_dictionary.Add(type, new(type));
            }
        }

        /// <summary>
        /// Insert an unsorted array of BodyPartInfo objects into the catalogue.
        /// </summary>
        public void InsertUnsorted(params BodyPartInfo[] _objects)
        {
            for (int i = 0; i < _objects.Length; i++)
            {
                m_dictionary[_objects[i].type].Insert(_objects[i]);
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Goosent
{
    public class ChannelsSetsList : IEnumerable<ChannelsSet>
    {
        private List<ChannelsSet> _setsList = new List<ChannelsSet>();

        public bool AddSet(ChannelsSet set)
        {
            // false если уже есть сет с таким именем
            foreach (ChannelsSet fSet in _setsList)
            {
                if (set.Name == fSet.Name)
                {
                    return false;
                }
            }
            _setsList.Add(set);

            return true;
        }

        public void AddChannel(Channel channel, int setIndex)
        {
            try
            {
                _setsList[setIndex].AddChannel(channel);
            } catch (Exception)
            {
                Console.WriteLine("Не удалось добавить канал из класса ChannelsSetsList");
            }

        }

        public void DeleteChannel(int setIndex, string channelName, DATA.Platforms platform)
        {
            try
            {
                _setsList[setIndex].DeleteChannel(channelName, platform);
            }
            catch (Exception)
            {
                Console.WriteLine("Не удалось добавить канал из класса ChannelsSetsList");
            }

        }

        public void DeleteSet(int setIndex)
        {
            try
            {
                _setsList.RemoveAt(setIndex);
            }
            catch (Exception)
            {
                Console.WriteLine("Не удалось удалить сет");
            }
        }

        public int Count {
            get { return _setsList.Count; }
            }

        public IEnumerator<ChannelsSet> GetEnumerator()
        {
            return _setsList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<ChannelsSet> GetSetsList
        {
            get { return _setsList; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using System.Threading.Tasks;
using Android.Support.V7.Widget;

namespace Goosent
{
    public class FragmentEditSets : BaseTabFragment
    {
        RecyclerView editSetRecyclerView;
        private Adapters.EditSetRecyclerViewAdapter editSetAdapter;
        public ChannelsSetsList _setList;
        private int _currentSetIndex;

        public static FragmentEditSets getInstance(Context context)
        {
            Bundle args = new Bundle();
            FragmentEditSets cFragment = new FragmentEditSets();
            cFragment.Arguments = args;
            cFragment.SetContext(context);
            cFragment.SetTitle(context.GetString(Resource.String.tab_item_edit_set));

            return cFragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.FragmentEditSetLayout, container, false);

            _currentSetIndex = ((MainActivity)Activity).SelectedSetIndex;
            _setList = ((MainActivity)Activity).SetsList;
            editSetRecyclerView = (RecyclerView)view.FindViewById(Resource.Id.editSet_recyclerView);
            editSetAdapter = new Adapters.EditSetRecyclerViewAdapter(Activity, _setList);
            editSetRecyclerView.SetLayoutManager(new LinearLayoutManager(context));
            editSetAdapter.ItemClick += EditSetAdapter_ItemClick;
            editSetAdapter.ItemLongClick += EditSetAdapter_ItemLongClick;
            editSetRecyclerView.SetAdapter(editSetAdapter);


            return view;

        }

        // Удалить сет
        private void EditSetAdapter_ItemLongClick(object sender, int e)
        {
            ((MainActivity)Activity).DeleteSet(e);
        }

        // Сделать сет выбранным
        private void EditSetAdapter_ItemClick(object sender, int e)
        {
            _currentSetIndex = e;

            // Смена индекса выбранного сета в мейн активити
            ((MainActivity)Activity).SelectedSetIndex = e;
            editSetAdapter.NotifyDataSetChanged();
        }

        public void UpdateEditSetListView()
        {
            //TODO: лист не обновляется при изменении текущего сета, а также при добавлении новых каналов. Исправить.
            _currentSetIndex = ((MainActivity)Activity).SelectedSetIndex;
            editSetAdapter.NotifyDataSetChanged();
        }

        public void SetContext(Context context)
        {
            this.context = context;
        }

    }
}
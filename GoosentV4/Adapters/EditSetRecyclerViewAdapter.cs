using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Android.Support.V7.Widget;
using Android.Support.Transitions;
using Android.Animation;

namespace Goosent.Adapters
{
    public class EditSetRecyclerViewAdapter : RecyclerView.Adapter
    {
        private List<ChannelsSet> _setsList;
        private Display display;

        private Context mContext;
        public event EventHandler<int> ItemClick;
        public event EventHandler<int> ItemLongClick;
        

        public EditSetRecyclerViewAdapter(Context context, ChannelsSetsList channelsSet)
        {
            mContext = context;
            _setsList = channelsSet.GetSetsList;
            display = ((Activity)mContext).WindowManager.DefaultDisplay;

        }


        public override int ItemCount
        {
            get { return _setsList.Count; }
        }

        void OnClick(int position)
        {
            if (ItemClick !=null)
            {
                ItemClick(this, position);
            }
        }

        void OnLongClick(int position)
        {
            if (ItemClick != null)
            {
                ItemLongClick(this, position);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            EditSetViewHolder viewHolder = (EditSetViewHolder)holder;
            viewHolder.setNameTextView.Text = _setsList[position].Name;
            // Если карта не была выбрана до этого, но стала выбранной сейчас
            if (!viewHolder.isSelected && ((MainActivity)mContext).SelectedSetIndex == position)
            {
                // Анимация "высоты" карточки
                var cardElevationValueAnimator = ValueAnimator.OfFloat(mContext.Resources.GetDimension(Resource.Dimension.setCardElevation), mContext.Resources.GetDimension(Resource.Dimension.setCardSelectedElevation));
                cardElevationValueAnimator.SetDuration(100);
                cardElevationValueAnimator.Update += (object sender, ValueAnimator.AnimatorUpdateEventArgs e) =>
                {
                    viewHolder.setCardView.CardElevation = (float)e.Animation.AnimatedValue;
                };
                cardElevationValueAnimator.Start();

                // Изменение цвета сепаратора
                viewHolder.separatorLine.SetBackgroundColor(mContext.Resources.GetColor(Resource.Color.primary));

                viewHolder.isSelected = true;
            }
            // Карта была выбранной, но сейчас стала обычной
            else if (viewHolder.isSelected && ((MainActivity)mContext).SelectedSetIndex != position)
            {
                // Анимация "высоты" карточки
                var cardElevationValueAnimator = ValueAnimator.OfFloat(mContext.Resources.GetDimension(Resource.Dimension.setCardSelectedElevation), mContext.Resources.GetDimension(Resource.Dimension.setCardElevation));
                cardElevationValueAnimator.SetDuration(100);
                cardElevationValueAnimator.Update += (object sender, ValueAnimator.AnimatorUpdateEventArgs e) =>
                {
                    viewHolder.setCardView.CardElevation = (float)e.Animation.AnimatedValue;
                };
                cardElevationValueAnimator.Start();

                // Изменение цвета сепаратора
                viewHolder.separatorLine.SetBackgroundColor(Android.Graphics.Color.Gray);

                viewHolder.isSelected = false;
            }

            viewHolder.channelsBaseLinearLayout.RemoveAllViews();

            int currentRowWidth = 0;
            bool startNewLine = true;
            int rowTotalWidth = 0;
            View viewInStack = null;
            LinearLayout rowLinearLayout = null;

            foreach (Channel channel in _setsList[position])
            {
                if (startNewLine)
                {
                    rowLinearLayout = GetNewRowLinearLayout();
                    viewHolder.channelsBaseLinearLayout.AddView(rowLinearLayout);
                    rowTotalWidth = GetWidthOfView(viewHolder.channelsBaseLinearLayout);

                    if (viewInStack != null) // Перенос вьюшки с предыдущей строки
                    {
                        rowLinearLayout.AddView(viewInStack);
                        currentRowWidth += GetWidthOfView(viewInStack);
                        viewInStack = null;
                    }
                    startNewLine = false;
                }
                View channelView = LayoutInflater.From(mContext).Inflate(Resource.Layout.SetCardView_ChannelView, null);            
                channelView.SetTag(Resource.String.channelViewNameStoringTag, channel.Name);    // Добавить информацию о имени канала и платформе в теги вьюшки
                channelView.SetTag(Resource.String.channelViewPlatformStoringTag, (int)channel.Platform);
                channelView.SetTag(Resource.String.channelViewSetIndexStoringTag, position);

                TextView channelTextView = (TextView)channelView.FindViewById(Resource.Id.setCardView_channelViewName_TextView);
                channelTextView.Text = channel.Name;

                currentRowWidth += GetWidthOfView(channelView);

                if (currentRowWidth >= display.Width * 0.9)
                {
                    startNewLine = true;
                    viewInStack = channelView;
                    currentRowWidth = 0;
                } else
                {
                    rowLinearLayout.AddView(channelView);
                }

                

                // Добавить синюю рамку каналам, если сет выделен
                if (((MainActivity)mContext).SelectedSetIndex == position)
                {
                    channelTextView.SetBackgroundDrawable(mContext.Resources.GetDrawable(Resource.Drawable.setCardView_channelSelectedViewBackground));
                } else
                {
                    channelTextView.SetBackgroundDrawable(mContext.Resources.GetDrawable(Resource.Drawable.setCardView_channelViewBackground));
                }

                channelView.LongClick += (object sender, View.LongClickEventArgs e) =>
                {
                    int setIndex = (int)channelView.GetTag(Resource.String.channelViewSetIndexStoringTag);
                    string channelName = (string)channelView.GetTag(Resource.String.channelViewNameStoringTag);
                    DATA.Platforms platform = (DATA.Platforms)(int)channelView.GetTag(Resource.String.channelViewPlatformStoringTag);

                    ((MainActivity)mContext).DeleteChannel(setIndex, channelName, platform);
                };
            }

            // Добавить кнопку добавления канала
            View addChannelView = LayoutInflater.From(mContext).Inflate(Resource.Layout.SetCardView_ChannelView, null);
            TextView addChannelTextView = (TextView)addChannelView.FindViewById(Resource.Id.setCardView_channelViewName_TextView);
            addChannelTextView.Text = mContext.Resources.GetString(Resource.String.add_channel_view_text);
            addChannelTextView.SetBackgroundColor(mContext.Resources.GetColor(Resource.Color.accentColor));
            addChannelTextView.SetTextColor(Android.Graphics.Color.White);
            currentRowWidth += GetWidthOfView(addChannelView);

            if (currentRowWidth >= display.Width * 0.9 || viewHolder.channelsBaseLinearLayout.ChildCount == 0)
            {
                rowLinearLayout = GetNewRowLinearLayout();
                viewHolder.channelsBaseLinearLayout.AddView(rowLinearLayout);
                rowLinearLayout.AddView(addChannelView);
            }
            else
            {
                rowLinearLayout.AddView(addChannelView);
            }

            addChannelView.Click += (object sender, EventArgs e) =>
            {
                Fragments.AddChatDialogFragment addChatDialogFragment = Fragments.AddChatDialogFragment.GetInstance(position);
                addChatDialogFragment.Show(((Activity)mContext).FragmentManager, "Adding chat to set " + position.ToString());
            };
        }

        private int GetWidthOfView(View view)
        {
            view.Measure(display.Width, display.Height);

            return view.MeasuredWidth;
        }

        private LinearLayout GetNewRowLinearLayout()
        {
            LinearLayout rowLinearLayout = new LinearLayout(mContext);
            rowLinearLayout.Orientation = Orientation.Horizontal;
            rowLinearLayout.SetGravity(GravityFlags.Left);
            rowLinearLayout.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.WrapContent);

            return rowLinearLayout;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(mContext).Inflate(Resource.Layout.SetCardView, parent, false);
            

            EditSetViewHolder vh = new EditSetViewHolder(view, OnClick, OnLongClick);

            return vh;
        }

        public class EditSetViewHolder : RecyclerView.ViewHolder
        {
            public TextView setNameTextView { get; set; }
            public LinearLayout channelsBaseLinearLayout { get; set; }
            public View separatorLine { get; set; }
            public CardView setCardView { get; set; }

            public bool isSelected { get; set; }

            public EditSetViewHolder(View itemView, Action<int> clickListener, Action<int> longClickListener) : base(itemView)
            {
                setNameTextView = (TextView)itemView.FindViewById(Resource.Id.setName_cardview_textView);
                channelsBaseLinearLayout = (LinearLayout)itemView.FindViewById(Resource.Id.setChannels_container_linearLayout);
                separatorLine = (View)itemView.FindViewById(Resource.Id.setChannels_separatorLine_view);
                setCardView = (CardView)itemView.FindViewById(Resource.Id.setCardView_cardView);
                isSelected = false;
                itemView.Click += (sender, e) => clickListener(base.LayoutPosition);
                itemView.LongClick += (sender, e) => longClickListener(base.LayoutPosition);
            }
        }
    }

    
}
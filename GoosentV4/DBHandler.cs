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
using Android.Database.Sqlite;
using Android.Database;

namespace Goosent
{
    /// <summary> 
    /// Класс работы с локальной базой данных
    /// </summary>
    class DBHandler : SQLiteOpenHelper
    {
        string DATABASE_NAME;
        string SETS_TABLE_NAME;
        string CHANNELS_TABLE_NAME;
        const string KEY_CHANNEL_ID = "id";
        const string KEY_CHANNEL_NAME = "channel_name";
        const string KEY_CHANNEL_PLATFORM = "platform";
        const string KEY_CHANNEL_SET_ID = "channel_set_id";
        const string KEY_SET_ID = "set_id";
        const string KEY_SET_NAME = "set_name";
        Context _context;

        
        public DBHandler(Context context) : base(context, context.Resources.GetString(Resource.String.database_name), null, 1)
        {
            _context = context;
            SETS_TABLE_NAME = _context.Resources.GetString(Resource.String.database_sets_table_name);
            CHANNELS_TABLE_NAME = _context.Resources.GetString(Resource.String.database_channels_table_name);
        }

        public override void OnCreate(SQLiteDatabase db)
        {   
            // Если база данных не найдена - создается новая, состаящая из двух таблиц для сетов и каналов
            db.ExecSQL("CREATE TABLE " + SETS_TABLE_NAME +
                " (" + KEY_SET_ID + " integer primary key autoincrement, "
                 + KEY_SET_NAME + " text)");

            db.ExecSQL("CREATE TABLE " + CHANNELS_TABLE_NAME +
                " (" + KEY_CHANNEL_ID + " integer primary key autoincrement, "
                 + KEY_CHANNEL_NAME + " text, "
                 + KEY_CHANNEL_PLATFORM + " text, "
                 + KEY_CHANNEL_SET_ID + " integer)");
        }
        
        /// <summary>
        /// Вызывается если поменялась версия БД (например изменилась структура и надо обновить всю БД), удаляя при этом старую версию
        /// </summary>
        /// <param name="db"></param>
        /// <param name="oldVersion"></param>
        /// <param name="newVersion"></param>
        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL("DROP TABLE IF EXISTS " + DATABASE_NAME);

            OnCreate(db);
        }

        /// <summary>
        /// Добавить канал в локальную БД
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="setIndex"></param>
        public void AddChannel(Channel channel, int setIndex)
        {
            SQLiteDatabase db = WritableDatabase;
            ContentValues values = new ContentValues();
            values.Put(KEY_CHANNEL_NAME, channel.Name);
            values.Put(KEY_CHANNEL_PLATFORM, (int)channel.Platform);
            values.Put(KEY_CHANNEL_SET_ID, setIndex + 1);

            var id = db.Insert(CHANNELS_TABLE_NAME, null, values);
            db.Close();
        }

        /// <summary>
        /// Удалить канал из локальной БД
        /// </summary>
        /// <param name="setIndex"></param>
        /// <param name="channelName"></param>
        /// <param name="channelPlatform"></param>
        public void DeleteChannel(int setIndex, string channelName, DATA.Platforms channelPlatform)
        {
            SQLiteDatabase db = WritableDatabase;
            var strs = new string[3] { channelName, ((int)channelPlatform).ToString(), (setIndex + 1).ToString() };
            db.Delete(CHANNELS_TABLE_NAME, KEY_CHANNEL_NAME + "=? and " + KEY_CHANNEL_PLATFORM + "=? and " + KEY_CHANNEL_SET_ID + "=?",strs );
        }

        /// <summary>
        /// Добавить сет в локальную БД
        /// </summary>
        /// <param name="set"></param>
        public void AddSet(ChannelsSet set)
        {
            SQLiteDatabase db = WritableDatabase;
            ContentValues sets_table_values = new ContentValues();

            sets_table_values.Put(KEY_SET_NAME, set.Name);
            var id = db.Insert(SETS_TABLE_NAME, null, sets_table_values);

            foreach (Channel channel in set.Channels)
            {
                AddChannel(channel, (int)id);
            }

            db.Close();
        }

        /// <summary>
        /// Удалить сет из локальной БД
        /// </summary>
        /// <param name="setIndex"></param>
        public void DeleteSet(string setName)
        {
            SQLiteDatabase db = WritableDatabase;
            db.Delete(SETS_TABLE_NAME, KEY_SET_NAME + "=?", new string[1] { setName });
        }

        /// <summary>
        /// Удалить канал из локальной БД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Channel GetChannel(int id)
        {
            SQLiteDatabase db = ReadableDatabase;
            ICursor cursor = db.Query(CHANNELS_TABLE_NAME, new string[] { KEY_CHANNEL_ID, KEY_CHANNEL_NAME, KEY_CHANNEL_PLATFORM, KEY_CHANNEL_SET_ID }, KEY_CHANNEL_ID + "=?", new string[] { id.ToString() }, null, null, null, null);
            if (cursor != null)
            {
                cursor.MoveToFirst();
            }

            // Канал найден
            if (cursor.Count > 0)
            {
                Channel channel = new Channel(cursor.GetString(1), (DATA.Platforms)Int32.Parse(cursor.GetString(2)));

                return channel;
            }


            return null;
        }

        /// <summary>
        /// Полностью очистить локальную БД
        /// </summary>
        public void ClearDatabase()
        {
            SQLiteDatabase db = WritableDatabase;
            try
            {
                db.Delete(SETS_TABLE_NAME, null, null);
            } catch (Exception) {

            };
            try
            {
                db.Delete(CHANNELS_TABLE_NAME, null, null);
            }
            catch (Exception)
            {

            };
            db.ExecSQL("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='" + CHANNELS_TABLE_NAME + "'");
            db.ExecSQL("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='" + SETS_TABLE_NAME + "'");
        }

        /// <summary>
        /// Отобразить содержимое локальной БД
        /// </summary>
        /// <returns></returns>
        public ChannelsSetsList GetDataFromDB()
        {
            ChannelsSetsList setsList = new ChannelsSetsList();
            SQLiteDatabase db;

            try
            {
                db = ReadableDatabase;
            } catch (Exception)
            {
                ClearDatabase();

                return setsList;
            }
            
            // Работа с сетами
            ICursor allSets = db.RawQuery("SELECT * FROM " + SETS_TABLE_NAME, null);
            if (allSets.MoveToFirst())
            {
                String[] columnNames = allSets.GetColumnNames();
                do
                {
                    // Проходимся по каждому сету в таблице сетов и создаем новый объект сета
                    var a = allSets.GetString(allSets.GetColumnIndex(columnNames[0]));
                    setsList.AddSet(new ChannelsSet(allSets.GetString(allSets.GetColumnIndex(columnNames[1]))));

                } while (allSets.MoveToNext());
            }

            // Работа с каналами
            ICursor allChannels = db.RawQuery("SELECT * FROM " + CHANNELS_TABLE_NAME, null);
            if (allChannels.MoveToFirst())
            {
                String[] columnNames = allChannels.GetColumnNames();
                do
                {
                    // Проходимся по каждому каналу в таблице каналов
                    var channelName = allChannels.GetString(allChannels.GetColumnIndex(columnNames[1]));
                    var channelPlatform = allChannels.GetString(allChannels.GetColumnIndex(columnNames[2]));
                    var channelSetIndex = allChannels.GetString(allChannels.GetColumnIndex(columnNames[3]));
                    Channel channel = new Channel(channelName, (DATA.Platforms)Int32.Parse(channelPlatform));
                    setsList.AddChannel(channel, Int32.Parse(channelSetIndex) - 1);

                } while (allChannels.MoveToNext());
            }

            return setsList;
        }

        public String GetTableAsString(String tableName)
         {
             SQLiteDatabase db = ReadableDatabase;
             String tableString = string.Format("Table {0}\n", tableName);
             ICursor allRows = db.RawQuery("SELECT * FROM " + tableName, null);
             if (allRows.MoveToFirst())
             {
                 String[] columnNames = allRows.GetColumnNames();
                 do
                 {
                     foreach (String name in columnNames)
                     {
                         tableString += string.Format("{0}: {1}\n", name,
                                 allRows.GetString(allRows.GetColumnIndex(name)));
                     }
                     tableString += "\n";
 
                 } while (allRows.MoveToNext());
             }
 
             return tableString;
          }

        //public void DeleteDatabase()
        //{
        //    try
        //    {
        //        _context.DeleteDatabase(DATABASE_NAME);
        //    } catch (Exception)
        //    {
        //        Console.WriteLine("Unable to delete database");
        //    }
            
        //}
    }
}
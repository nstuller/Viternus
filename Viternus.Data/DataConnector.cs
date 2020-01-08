using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Viternus.Data
{
    public static class DataConnector
    {
        private static ViternusEntities _context;

        //Was using a singleton from my WPF experience. It doesn't work well with asp.net
        //Now using a singleton that is set per request (had to add a setter to enable this)
        public static ViternusEntities Context
        {
            get
            {
                if (null != HttpContext.Current)
                {
                    string objectContextKey = HttpContext.Current.GetHashCode().ToString("x");
                    if (!HttpContext.Current.Items.Contains(objectContextKey))
                    {
                        HttpContext.Current.Items.Add(objectContextKey, new ViternusEntities());
                    }
                    return HttpContext.Current.Items[objectContextKey] as ViternusEntities;
                }
                else
                {
                    //We want to use a new ObjectContext if it's null or the Connection has been disposed
                    try
                    {
                        if (null == _context || null == _context.Connection)
                        {
                            _context = new ViternusEntities();
                        }
                    }
                    catch (ObjectDisposedException odex)
                    {
                        _context = new ViternusEntities();
                    }

                    return _context;
                }
            }

            //set
            //{
            //    _context = value;
            //}
        }

        public static void CleanUp()
        {
            if (null != _context)
            {
                _context.Dispose();
            }
        }

        //public DataConnector()
        //{

        //}
    }
}

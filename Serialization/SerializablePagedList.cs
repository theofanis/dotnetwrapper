﻿using System;
using Newtonsoft.Json;

namespace Moosend.API.Client.Serialization
{
    internal class SerializablePagedList<T>
    {
        private PagedList<T> _PagedList;

        [JsonProperty]
        public PagedList<T> PagedList
        {
            get
            {
                // update properties of PagedList from the deserialized properties of SerializablePagedList 
                // before returning the expected PagedList object to the user
                UpdateList();
                return _PagedList;
            }
            set
            {
                _PagedList = value;
            }
        }

        [JsonProperty("Paging")]
        public SerializablePagingInfo PagingInfo
        {
            get;
            set;
        }

        private void UpdateList()
        {
            if (_PagedList != null && this.PagingInfo != null)
            {
                if (_PagedList.PagingInfo != null)
                {
                    _PagedList.PagingInfo.CurrentPage = this.PagingInfo.CurrentPage;
                    _PagedList.PagingInfo.PageSize = this.PagingInfo.PageSize;
                    _PagedList.PagingInfo.SortExpression = this.PagingInfo.SortExpression;
                    _PagedList.PagingInfo.SortIsAscending = this.PagingInfo.SortIsAscending;
                }
                _PagedList.TotalResults = this.PagingInfo.TotalResults;
            }
        }

    }
}
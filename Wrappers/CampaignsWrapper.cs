﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moosend.API.Client.Models;
using Moosend.API.Client.Serialization;

namespace Moosend.API.Client.Wrappers
{
    public class CampaignsWrapper
    {
        private ApiManager _Manager;

        internal CampaignsWrapper(ApiManager manager)
        {
            _Manager = manager;
        }

        public Guid Create(CampaignParams campaignParams)
        {
            return _Manager.MakeRequest<Guid>(HttpMethod.POST, "/campaigns/create", campaignParams);
        }

        public Campaign Clone(Guid campaignID)
        {
            return _Manager.MakeRequest<Campaign>(HttpMethod.POST, "/campaigns/clone", new { CampaignID = campaignID });
        }

        public void SendTest(Guid campaignID, IList<String> emails)
        {
            _Manager.MakeRequest(HttpMethod.POST, String.Format("/campaigns/{0}/send_test", campaignID), emails);
        }

        public void Send(Guid campaignID)
        {
            _Manager.MakeRequest(HttpMethod.POST, String.Format("/campaigns/{0}/send", campaignID));
        }

        public void Update(Campaign campaign)
        {
            CampaignParams cp = new CampaignParams()
            {
                Name = campaign.Name,
                Subject = campaign.Subject,
                WebLocation = campaign.WebLocation,
                ConfirmationToEmail = campaign.ConfirmationTo
            };

            if (campaign.Sender != null) cp.SenderEmail = campaign.Sender.Email;
            else cp.SenderEmail = null;

            if (campaign.ReplyToEmail != null) cp.ReplyToEmail = campaign.ReplyToEmail.Email;
            else cp.ReplyToEmail = null;

            if (campaign.MailingList != null) cp.MailingListID = campaign.MailingList.ID;
            else cp.MailingListID = Guid.Empty;

            if (campaign.Segment != null) cp.SegmentID = campaign.Segment.ID;
            else cp.SegmentID = 0;

            if (campaign.ABCampaignData != null)
            {
                cp.ABCampaignType = campaign.ABCampaignData.ABCampaignType;
                cp.ABWinnerSelectionType = campaign.ABCampaignData.ABWinnerSelectionType;
                cp.HoursToTest = campaign.ABCampaignData.HoursToTest;
                cp.ListPercentage = campaign.ABCampaignData.ListPercentage;
                cp.SubjectB = campaign.ABCampaignData.SubjectB;
                cp.WebLocationB = campaign.ABCampaignData.WebLocationB;

                if (campaign.ABCampaignData.SenderB != null) cp.SenderEmailB = campaign.ABCampaignData.SenderB.Email;
                else cp.SenderEmailB = null;
            }
            _Manager.MakeRequest(HttpMethod.POST, String.Format("/campaigns/{0}/update", campaign.ID), cp);
        }

        public void Delete(Guid campaignID)
        {
            _Manager.MakeRequest(HttpMethod.POST, String.Format("/campaigns/{0}/delete", campaignID));
        }

        public Campaign FindByID(Guid campaignID)
        {
            return _Manager.MakeRequest<Campaign>(HttpMethod.GET, String.Format("/campaigns/{0}/view", campaignID));
        }

        public CampaignStatisticsSummary FindByIDWithStatistics(Guid campaignID)
        {
            return _Manager.MakeRequest<CampaignStatisticsSummary>(HttpMethod.GET, String.Format("/campaigns/{0}/view_summary", campaignID));
        }

        public PagedList<ContextAnalyticsNode> GetGeographicsActivity(Guid campaignID)
        {
            return _Manager.MakeRequest<SerializableContextAnalyticsNodeCollection>(HttpMethod.GET, String.Format("/campaigns/{0}/stats/countries", campaignID)).PagedList;
        }

        public PagedList<ContextAnalyticsNode> GetLinksActivity(Guid campaignID)
        {
            return _Manager.MakeRequest<SerializableContextAnalyticsNodeCollection>(HttpMethod.GET, String.Format("/campaigns/{0}/stats/links", campaignID)).PagedList;
        }

        public PagedList<ContextAnalyticsNode> GetEventsByType(Guid campaignID, MailStatus type = MailStatus.Sent, int page = 1, int pageSize = 50, DateTime? from = null, DateTime? to = null)
        {
            return _Manager.MakeRequest<SerializableContextAnalyticsNodeCollection>(HttpMethod.GET, String.Format("/campaigns/{0}/stats/{1}", campaignID, type.ToString())).PagedList;
        }

        public PagedList<CampaignSummary> FindAll(int page = 1, int pageSize = 10)
        {
            return _Manager.MakeRequest<SerializableCampaignSummaryCollection>(HttpMethod.GET, String.Format("/campaigns/{0}/{1}", page, pageSize)).PagedList;
        }

    }
}

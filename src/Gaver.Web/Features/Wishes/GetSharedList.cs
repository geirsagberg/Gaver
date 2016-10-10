using System.Linq;
using AutoMapper.QueryableExtensions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic.Contracts;

namespace Gaver.Web.Features.Wishes
{
    public class GetSharedListRequest
    {
        public int ListId { get; set; }
    }
}
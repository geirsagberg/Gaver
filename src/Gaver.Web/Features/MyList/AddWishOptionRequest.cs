﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Gaver.Web.Contracts;
using Gaver.Web.Features.Shared.Models;
using HybridModelBinding;
using MediatR;

namespace Gaver.Web.Features.MyList;

public class AddWishOptionRequest : IRequest<WishOptionDto>, IMyWishRequest
{
    [JsonIgnore]
    public int UserId { get; set; }

    [HybridBindProperty(Source.Route)]
    [JsonIgnore]
    public int WishId { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(64)]
    public string Title { get; set; } = "";
    public string? Url { get; set; }
}
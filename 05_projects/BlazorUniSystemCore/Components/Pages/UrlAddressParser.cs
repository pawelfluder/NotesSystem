using System.Text.RegularExpressions;
using BlazorUniSystemCore.Registrations;
using BlazorUniSystemCore.Repet.Models;
using SharpOperationsProg.AAPublic;
using SharpRepoServiceProg.AAPublic;

namespace BlazorUniSystemCore.Components.Pages;

public class UrlAddressParser
{
    private string sptr = "-";
    private IRepoService _repo;
    //private readonly IOperationsService _operations;
    private readonly IRepoOperationsService _repoOp;

    public UrlAddressParser()
    {
        _repo = MyBorder.OutContainer.Resolve<IRepoService>();
        _repoOp = MyBorder.OutContainer.Resolve<IRepoOperationsService>();
        //_operations = MyBorder.OutContainer.Resolve<IOperationsService>();
    }

    public bool IsItemReloadNeeded(
        ItemModel currentItem,
        ItemModel newItem)
    {
        if (newItem.AdrTuple == default) { return false; }
        if (currentItem.AdrTuple != newItem.AdrTuple) { return true; }
        return false;
    }

    public bool IsPageReloadNeeded(
        ItemModel item,
        string browserCurrentUrl,
        out string newUrl)
    {
        if (item.AdrTuple == default)
        {
            newUrl = string.Empty;
            return false;
        }
        
        newUrl = _repoOp.Address
            .CreateUrl(item.AdrTuple);
        if (newUrl == browserCurrentUrl)
        {
            return false;
        }

        return true;
    }
    
    public bool GetItem(
        ref ItemModel item,
        string? UrlUniAddress)
    {
        if (string.IsNullOrEmpty(UrlUniAddress) ||
            UrlUniAddress.EndsWith(".css"))
        {
            return false;
        }
        
        bool s05 = false;
        if (UrlUniAddress == null)
        {
            var firstRepoAdrTyple = _repo.Methods.GetFirstRepo();
            s05 = _repoOp.Item.GetItem(ref item, firstRepoAdrTyple);
            if (s05) { return true; }
        }

        bool s06 = IsRepo(UrlUniAddress), s07 = false;
        if (s06)
        {
            s07 = _repoOp.Item.GetItem(ref item, (UrlUniAddress,""));
            if (s07) { return true; }
        }
        
        bool s01 = IsRepoQGuid(UrlUniAddress, out string repoName, out Guid guid),
        s02 = false;
        if (s01)
        {
            s02 = _repoOp.Item.GetByGuid(ref item, repoName, guid);
            if (s02) { return true; }
        }
        
        bool s03 = IsRepoAddress(UrlUniAddress, out (string, string) adrTuple),
        s04 = false;
        if (s03)
        {
            s04 = _repoOp.Item.GetItem(ref item, adrTuple);
        }
        
        if (s04)
        {
            return true;
        }
        
        return false;
    }

    private bool HandleGuid(
        ref ItemModel item,
        string repoName,
        Guid guid)
    {
        bool s01 = _repoOp.Item.GetByGuid(ref item,repoName, guid);
        if (!s01)
        {
            return false;
        }

        return true;
    }

    private bool IsRepo(
        string? urlUniAddress)
    {
        if (urlUniAddress.Contains('-') ||
            urlUniAddress.Contains('/') ||
            string.IsNullOrEmpty(urlUniAddress))
        {
            return false;
        }

        return true;
    }

    private bool IsRepoQGuid(
        string? urlUniAddress,
        out string outRepoName,
        out Guid outGuid)
    {
        outRepoName = default;
        outGuid = default;
        string pattern = @"^([A-Za-z0-9]{1,13})-([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})$";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(urlUniAddress);
        bool correct = match.Success;
        
        if (!correct)
        {
            return false;
        }
        
        string guidStr = match.Groups[2].Value;
        
        bool isGuid = Guid.TryParse(guidStr, out Guid guid);
        if (isGuid)
        {
            outRepoName = match.Groups[1].Value;
            outGuid = guid;
            return true;
        }
        
        return false;
    }

    private bool IsRepoAddress(
        string? urlUniAddress,
        out (string Repo, string Loca) adrTuple)
    {
        adrTuple = default;
        string pattern = @"^([A-Za-z0-9]{1,13})-((0[0-9]|[1-9][0-9]|[1-9][0-9]{2})(-[0-9]{2,3})*)$";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(urlUniAddress);
        bool correct = match.Success;
        if (!match.Success)
        {
            return false;
        }

        string loca = match.Groups[2].Value.Replace('-', '/');
        
        adrTuple = (match.Groups[1].Value, loca);
        return true;
    }
}

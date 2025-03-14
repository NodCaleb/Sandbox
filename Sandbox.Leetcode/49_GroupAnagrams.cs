namespace Sandbox.Leetcode;

public class _49_GroupAnagrams
{
    public static IEnumerable<object[]> GetTestData()
    {
        yield return new object[] { new string[] { "eat", "tea", "tan", "ate", "nat", "bat" }, 3 };
        yield return new object[] { new string[] { "1" }, 1 };
        yield return new object[] { new string[] { "" }, 1 };
    }


    [Theory]
    [MemberData(nameof(GetTestData))]
    public void Group(string[] strs, int groupsCount)
    {
        var result = GroupAnagrams(strs);
        Assert.Equal(groupsCount, result.Count);
    }

    private IList<IList<string>> GroupAnagrams(string[] strs)
    {
        var groups = new List<IList<string>>();

        foreach (var str in strs)
        {
            bool addedToGroup = false;

            foreach (var group in groups)
            {
                if (IsAnagram(str, group.First()))
                {
                    group.Add(str);
                    addedToGroup = true;
                    break;
                }
            }

            if (!addedToGroup)
            {
                groups.Add(new List<string> { str });
            }
        }

        return groups;
    }

    private bool IsAnagram(string a, string b)
    {
        if (a.Length != b.Length)
            throw new ArgumentException();

        var arrA = a.ToArray();
        Array.Sort(arrA);
        var arrB = b.ToArray();
        Array.Sort(arrB);

        for (int i = 0; i < arrA.Length; i++)
        {
            if (arrA[i] != arrB[i])
                return false;
        }

        return true;
    }

}

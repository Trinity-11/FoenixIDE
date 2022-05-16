using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FoenixIDE.GameGenerator
{
    public enum TokenType
    {
        ASSET,
        COPY,
        SCOPY,
        IOCOPY,
        ASSIGNMENT,
        SUB,
        LABEL,
        GOTO,
        FILL,
        VGM_INIT,
        VGM_PLAY,
        ENABLE_INTERRUPTS,
        // sprites
        ENABLE_SPRITE,
        DISABLE_SPRITE,
        SET_SPRITE_POS,
        // bitmaps
        ENABLE_BITMAP,
        DISABLE_BITMAP,
        // tilemaps
        ENABLE_TILEMAP,
        DISABLE_TILEMAP,
        SET_TILEMAP_POS,
        IF,
        VAR,
        INCR,
        DECR,
        VAR_INCR,
        IF_ELSE,
        COMMENT
    }

    public class TokenMatch
    {
        public TokenType TokenType { get; set; }
        public string Value { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int Precedence { get; set; }

        public List<string> groups;
    }

    public class TokenDefinition
    {
        private Regex _regex;
        private readonly TokenType _returnsToken;
        private readonly int _precedence;

        public TokenDefinition(TokenType returnsToken, string regexPattern, int precedence)
        {
            _regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
            _returnsToken = returnsToken;
            _precedence = precedence;
        }

        public IEnumerable<TokenMatch> FindMatches(string inputString)
        {
            var matches = _regex.Matches(inputString);
            for (int i = 0; i < matches.Count; i++)
            {
                yield return new TokenMatch()
                {
                    StartIndex = matches[i].Index,
                    EndIndex = matches[i].Index + matches[i].Length,
                    TokenType = _returnsToken,
                    Value = matches[i].Value,
                    Precedence = _precedence,
                    groups = GroupsToString(matches[i].Groups)
                };
            }
        }

        private List<string> GroupsToString(GroupCollection mGroups)
        {
            List<string> g = new List<string>();
            for (int i=1;i< mGroups.Count;i++)
            {
                string val = mGroups[i].Value;
                // Replace : in Hex values, like $B0:1234 to $B0_1234
                if (val.StartsWith("$") && val.Contains(":"))
                {
                    val = val.Replace(":", "_");
                }
                g.Add(val);
            }
            return g;
        }
    }

    public class Asset
    {
        public string label;
        public string filename;
    }

    public class FoenixLexer
    {
        private readonly List<Asset> assets = new List<Asset>();
        private readonly Dictionary<string, List<TokenMatch>> subs = new Dictionary<string, List<TokenMatch>>();
        private readonly List<TokenDefinition> _tokenDefinitions = new List<TokenDefinition>();
        private readonly List<TokenMatch> tokenMatches = new List<TokenMatch>();

        public FoenixLexer(string code)
        {
            // in multiline regex, the $ catches \n -  be careful
            _tokenDefinitions.Add(new TokenDefinition(TokenType.COMMENT, @"(?s)/\*.*\*/", 1));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ASSET, @"^[ \t]*asset\s+("".+""|\S+)\s+(\w+)(\s*//.*^\r)?", 1));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.SUB, @"(\S+)\s*{((?>[^{}]+|{(?<c>)|}(?<-c>))*(?(c)(?!)))}", 1));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.COPY, @"^[ \t]*copy\s+(\S*)\s+(\S*)\s+(\S*)(\s*//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ASSIGNMENT, @"^[ \t]*(?!var\s+)(\w+)\s*=\s*(\S*)(\s*//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.VAR, @"^[ \t]*var\s+(\w+)\s*=\s*(\S*)(\s*//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.LABEL, @"^[ \t]*(\w+)\s*:(\s*//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.GOTO, @"^[ \t]*goto\s+(\S*)(\s//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.INCR, @"^[ \t]*incr\s+(\S*)(\s//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.DECR, @"^[ \t]*decr\s+(\S*)(\s//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.FILL, @"^[ \t]*fill\s+(\S*)\s+(\S*)\s+(\S*)(\s*//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.VGM_INIT, @"^[ \t]*vgm_init\s+(\S*)(\s*//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.VGM_PLAY, @"^[ \t]*vgm_play(\s*//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ENABLE_INTERRUPTS, @"^[ \t]*enable_interrupts(\s*//.*^\r)?", 2));
            // sprites
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ENABLE_SPRITE, @"^[ \t]*enable_sprite\s+([0-6]?[0-9]{1})\s+([0-7])\s+([0-6])\s+(\S+)(\s*//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.DISABLE_SPRITE, @"^[ \t]*disable_sprite\s+([0-6]?[0-9]{1})(\s*//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.SET_SPRITE_POS, @"^[ \t]*set_sprite_pos\s+([0-6]?[0-9]{1})\s+(\S+)\s+(\S+)(\s*//.*^\r)?", 2));
            // bitmaps
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ENABLE_BITMAP, @"^[ \t]*enable_bitmap\s+([0-1]{1})\s+([0-7]{1})\s+(\S+)(\s*//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.DISABLE_BITMAP, @"^[ \t]*disable_bitmap\s+([0-1]{1})(\s*//.*^\r)?", 2));
            // tilemaps
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ENABLE_TILEMAP, @"^[ \t]*enable_tilemap\s+([0-4]{1})\s+(\S+)\s+(\S+)\s+(\S+)(\s*//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ENABLE_TILEMAP, @"^[ \t]*disable_tilemap\s+([0-4]{1})(\s*//.*^\r)?", 2));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.SET_TILEMAP_POS, @"^[ \t]*set_tilemap_pos\s+([0-4]{1})\s+(\S+)\s+(\S+)(\s*//.*^\r)?", 2));



            foreach (TokenDefinition td in _tokenDefinitions)
            {
                tokenMatches.AddRange(td.FindMatches(code).ToList());
            }
            
            foreach (TokenMatch tm in tokenMatches.OrderBy(x => x.StartIndex).ToList())
            {
                switch (tm.TokenType)
                {
                    case TokenType.ASSET:
                        assets.Add(new Asset()
                        {
                            label = tm.groups[1],
                            filename = tm.groups[0]
                        });
                        tokenMatches.Remove(tm);
                        break;
                    case TokenType.COMMENT:
                        tokenMatches.Remove(tm);
                        break;
                    case TokenType.COPY:
                        try
                        {
                            int dest_addr = Convert.ToInt32(tm.groups[1].Replace("$", "").Replace("_", ""), 16);
                            if (dest_addr < 0xB0_0000)
                            {
                                tm.TokenType = TokenType.SCOPY;
                            }
                        }
                        catch(Exception)
                        {
                            // Lookup the token for an address
                        }
                        break;
                    case TokenType.SUB:
                        
                        tokenMatches.Remove(tm);
                        // Move tokens that are between start and end index
                        List<TokenMatch> functionSub = tokenMatches.FindAll(m => m.StartIndex > tm.StartIndex && m.EndIndex < tm.EndIndex);
                        if (!subs.ContainsKey(tm.groups[0]))
                        {
                            subs.Add(tm.groups[0], functionSub);
                        }
                        else
                        {
                            subs.Add(tm.groups[0] + "_dup", functionSub);
                        }
                        tokenMatches.RemoveAll(m => m.StartIndex > tm.StartIndex && m.EndIndex < tm.EndIndex);
                        break;
                }
            }
            // We need to sort by the start index in order to ensure that the code is run in the correct order.
            tokenMatches = tokenMatches.OrderBy(x => x.StartIndex).ToList();
        }

        /**
         * An assets is described by "ASSET" keyword, then the file name and an address label
         */
        public List<Asset> GetAssets()
        {
            return assets;
        }

        public List<TokenMatch> GetCode()
        {
            return tokenMatches;
        }

        public List<TokenMatch> GetSub(string fname)
        {
            List<TokenMatch> result;
            subs.TryGetValue(fname, out result);
            if (result != null)
            {
                result = result.OrderBy(x => x.StartIndex).ToList();
            }
            return result;
        }
    }
}

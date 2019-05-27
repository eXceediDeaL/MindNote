using Microsoft.Extensions.DependencyInjection;
using MindNote.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNote.API.Database
{
    public static class SeedData
    {
        public static void Initialize(Data.Providers.SqlServer.Models.DataContext context)
        {
            var rand = new Random();

            if (context.Nodes.Any())
                return;

            var ts = new List<Tag>();
            for (int i = 1; i < 7; i++)
            {
                Tag cn = new Tag
                {
                    Name = $"tag{i}",
                    Color = "black",
                };
                ts.Add(cn);
            }

            var ns = new List<Node>();
            for (int i = 1; i < 7; i++)
            {
                Node cn = new Node
                {
                    Name = $"Note {i}",
                    Content = $"content {i}",
                    CreationTime = DateTimeOffset.Now,
                    ModificationTime = DateTimeOffset.Now,
                    Tags = new Tag[] { new Tag { Id = i } },
                    Extra = Newtonsoft.Json.JsonConvert.SerializeObject(new { x = rand.Next(100), y = rand.Next(100) }),
                };
                ns.Add(cn);
            }

            var rs = new List<Relation>();
            for (int i = 1; i < 7; i++)
            {
                Relation cn = new Relation
                {
                    From = i == 1 ? 6 : i - 1,
                    To = i,
                    Color = "grey",
                };
                rs.Add(cn);
            }

            var ss = new List<Struct>();
            for (int i = 1; i < 7; i++)
            {
                Struct cn = new Struct
                {
                    Name = $"Struct {i}",
                    CreationTime = DateTimeOffset.Now,
                    ModificationTime = DateTimeOffset.Now,
                    Relations = new Relation[] { new Relation { Id = i } },
                    Tags = new Tag[] { new Tag { Id = i } },
                    Extra = Newtonsoft.Json.JsonConvert.SerializeObject(new { color = "blue" }),
                };
                ss.Add(cn);
            }

            foreach (var v in ns)
                context.Nodes.Add(Data.Providers.SqlServer.Models.Node.FromModel(v));

            foreach (var v in ss)
                context.Structs.Add(Data.Providers.SqlServer.Models.Struct.FromModel(v));

            foreach (var v in rs)
                context.Relations.Add(Data.Providers.SqlServer.Models.Relation.FromModel(v));

            foreach (var v in ts)
                context.Tags.Add(Data.Providers.SqlServer.Models.Tag.FromModel(v));

            context.SaveChanges();

            {
                var tag = Data.Providers.SqlServer.Models.Tag.FromModel(new Tag
                {
                    Color = "green",
                    Name = "Python",
                });
                context.Tags.Add(tag);
                context.SaveChanges();

                var nds = new List<Data.Providers.SqlServer.Models.Node>();

                foreach (var v in RealNote)
                {
                    var nd = Data.Providers.SqlServer.Models.Node.FromModel(new Node
                    {
                        Name = v.Item1,
                        Content = System.Text.Encoding.Default.GetString(Convert.FromBase64String(v.Item2)),
                        Tags = new Tag[] { new Tag { Id = tag.Id } },
                        CreationTime = DateTimeOffset.Now,
                        Extra = Newtonsoft.Json.JsonConvert.SerializeObject(new { x = rand.Next(100), y = rand.Next(100) }),
                    });
                    nds.Add(nd);
                }
                foreach (var v in nds) context.Nodes.Add(v);
                context.SaveChanges();

                var rls = new List<Data.Providers.SqlServer.Models.Relation>();
                rls.Add(Data.Providers.SqlServer.Models.Relation.FromModel(new Relation
                {
                    Color = "grey",
                    From = nds[0].Id,
                    To = nds[1].Id,
                }));
                rls.Add(Data.Providers.SqlServer.Models.Relation.FromModel(new Relation
                {
                    Color = "grey",
                    From = nds[1].Id,
                    To = nds[2].Id,
                }));
                foreach (var v in rls) context.Relations.Add(v);
                context.SaveChanges();

                var str = Data.Providers.SqlServer.Models.Struct.FromModel(new Struct
                {
                    CreationTime = DateTimeOffset.Now,
                    ModificationTime = DateTimeOffset.Now,
                    Name = "Python 100 Days",
                    Tags = new Tag[] { new Tag { Id = tag.Id } },
                    Relations = rls.Select(x => new Relation { Id = x.Id }).ToArray(),
                    Extra = Newtonsoft.Json.JsonConvert.SerializeObject(new { color = "orange" }),
                });
                context.Structs.Add(str);
                context.SaveChanges();
            }
        }

        static readonly (string, string)[] RealNote = new (string, string)[]{("Day01 - 初识Python", "IyMgRGF5MDEgLSDliJ3or4ZQeXRob24KCiMjIyBQeXRob27nroDku4sKCiMjIyMgUHl0aG9u55qE5Y6G5Y+yCgoxLiAxOTg55bm05Zyj6K+e6IqC77yaR3VpZG8gdm9uIFJvc3N1beW8gOWni+WGmVB5dGhvbuivreiogOeahOe8luivkeWZqOOAggoyLiAxOTkx5bm0MuaciO+8muesrOS4gOS4qlB5dGhvbue8luivkeWZqO+8iOWQjOaXtuS5n+aYr+ino+mHiuWZqO+8ieivnueUn++8jOWug+aYr+eUqEPor63oqIDlrp7njrDnmoTvvIjlkI7pnaLlj4jlh7rnjrDkuoZKYXZh5ZKMQyPlrp7njrDnmoTniYjmnKxKeXRob27lkoxJcm9uUHl0aG9u77yM5Lul5Y+KUHlQeeOAgUJyeXRob27jgIFQeXN0b27nrYnlhbbku5blrp7njrDvvInvvIzlj6/ku6XosIPnlKhD6K+t6KiA55qE5bqT5Ye95pWw44CC5Zyo5pyA5pep55qE54mI5pys5Lit77yMUHl0aG9u5bey57uP5o+Q5L6b5LqG5a+54oCc57G74oCd77yM4oCc5Ye95pWw4oCd77yM4oCc5byC5bi45aSE55CG4oCd562J5p6E6YCg5Z2X55qE5pSv5oyB77yM5ZCM5pe25o+Q5L6b5LqG4oCc5YiX6KGo4oCd5ZKM4oCc5a2X5YW44oCd562J5qC45b+D5pWw5o2u57G75Z6L77yM5ZCM5pe25pSv5oyB5Lul5qih5Z2X5Li65Z+656GA55qE5ouT5bGV57O757uf44CCCjMuIDE5OTTlubQx5pyI77yaUHl0aG9uIDEuMOato+W8j+WPkeW4g+OAggo0LiAyMDAw5bm0MTDmnIgxNuaXpe+8mlB5dGhvbiAyLjDlj5HluIPvvIzlop7liqDkuoblrp7njrDlrozmlbTnmoRb5Z6D5Zy+5Zue5pS2XShodHRwczovL3poLndpa2lwZWRpYS5vcmcvd2lraS8lRTUlOUUlODMlRTUlOUMlQkUlRTUlOUIlOUUlRTYlOTQlQjZfKCVFOCVBOCU4OCVFNyVBRSU5NyVFNiVBOSU5RiVFNyVBNyU5MSVFNSVBRCVCOCkp77yM5o+Q5L6b5LqG5a+5W1VuaWNvZGVdKGh0dHBzOi8vemgud2lraXBlZGlhLm9yZy93aWtpL1VuaWNvZGUp55qE5pSv5oyB44CC5LiO5q2k5ZCM5pe277yMUHl0aG9u55qE5pW05Liq5byA5Y+R6L+H56iL5pu05Yqg6YCP5piO77yM56S+5Yy65a+55byA5Y+R6L+b5bqm55qE5b2x5ZON6YCQ5riQ5omp5aSn77yM55Sf5oCB5ZyI5byA5aeL5oWi5oWi5b2i5oiQ44CCCjUuIDIwMDjlubQxMuaciDPml6XvvJpQeXRob24gMy4w5Y+R5biD77yM5a6D5bm25LiN5a6M5YWo5YW85a655LmL5YmN55qEUHl0aG9u5Luj56CB77yM5LiN6L+H5Zug5Li655uu5YmN6L+Y5pyJ5LiN5bCR5YWs5Y+45Zyo6aG555uu5ZKM6L+Q57u05Lit5L2/55SoUHl0aG9uIDIueOeJiOacrO+8jOaJgOS7pVB5dGhvbiAzLnjnmoTlvojlpJrmlrDnibnmgKflkI7mnaXkuZ/ooqvnp7vmpI3liLBQeXRob24gMi42LzIuN+eJiOacrOS4reOAggoK55uu5YmN5oiR5Lus5L2/55So55qEUHl0aG9uIDMuNy5455qE54mI5pys5piv5ZyoMjAxOOW5tOWPkeW4g+eahO+8jFB5dGhvbueahOeJiOacrOWPt+WIhuS4uuS4ieaute+8jOW9ouWmgkEuQi5D44CC5YW25LitQeihqOekuuWkp+eJiOacrOWPt++8jOS4gOiIrOW9k+aVtOS9k+mHjeWGme+8jOaIluWHuueOsOS4jeWQkeWQjuWFvOWuueeahOaUueWPmOaXtu+8jOWinuWKoEHvvJtC6KGo56S65Yqf6IO95pu05paw77yM5Ye6546w5paw5Yqf6IO95pe25aKe5YqgQu+8m0PooajnpLrlsI/nmoTmlLnliqjvvIjlpoLkv67lpI3kuobmn5DkuKpCdWfvvInvvIzlj6ropoHmnInkv67mlLnlsLHlop7liqBD44CC5aaC5p6c5a+5UHl0aG9u55qE5Y6G5Y+y5oSf5YW06Laj77yM5Y+v5Lul5p+l55yL5LiA56+H5ZCN5Li6W+OAilB5dGhvbueugOWPsuOAi10oaHR0cDovL3d3dy5jbmJsb2dzLmNvbS92YW1laS9hcmNoaXZlLzIwMTMvMDIvMDYvMjg5MjYyOC5odG1sKeeahOWNmuaWh+OAggoK"),
            ("Day02 - 语言元素", "IyMgRGF5MDIgLSDor63oqIDlhYPntKAKCiMjIyMg5oyH5Luk5ZKM56iL5bqPCgrorqHnrpfmnLrnmoTnoazku7bns7vnu5/pgJrluLjnlLHkupTlpKfpg6jku7bmnoTmiJDvvIzljIXmi6zvvJrov5DnrpflmajjgIHmjqfliLblmajjgIHlrZjlgqjlmajjgIHovpPlhaXorr7lpIflkozovpPlh7rorr7lpIfjgILlhbbkuK3vvIzov5DnrpflmajlkozmjqfliLblmajmlL7lnKjkuIDotbflsLHmmK/miJHku6zpgJrluLjmiYDor7TnmoTkuK3lpK7lpITnkIblmajvvIzlroPnmoTlip/og73mmK/miafooYzlkITnp43ov5DnrpflkozmjqfliLbmjIfku6Tku6Xlj4rlpITnkIborqHnrpfmnLrova/ku7bkuK3nmoTmlbDmja7jgILmiJHku6zpgJrluLjmiYDor7TnmoTnqIvluo/lrp7pmYXkuIrlsLHmmK/mjIfku6TnmoTpm4blkIjvvIzmiJHku6znqIvluo/lsLHmmK/lsIbkuIDns7vliJfnmoTmjIfku6TmjInnhafmn5Dnp43mlrnlvI/nu4Tnu4fliLDkuIDotbfvvIznhLblkI7pgJrov4fov5nkupvmjIfku6TljrvmjqfliLborqHnrpfmnLrlgZrmiJHku6zmg7PorqnlroPlgZrnmoTkuovmg4XjgILku4rlpKnmiJHku6zkvb/nlKjnmoTorqHnrpfmnLromb3nhLblmajku7blgZrlt6XotormnaXotornsr7lr4bvvIzlpITnkIbog73lipvotormnaXotorlvLrlpKfvvIzkvYbnqbblhbbmnKzotKjmnaXor7Tku43nhLblsZ7kuo5b4oCc5Yavwrfor7rkvp3mm7znu5PmnoTigJ1dKGh0dHBzOi8vemgud2lraXBlZGlhLm9yZy93aWtpLyVFNSU4NiVBRiVDMiVCNyVFOCVBRiVCQSVFNCVCQyU4QSVFNiU5QiVCQyVFNyVCQiU5MyVFNiU5RSU4NCnnmoTorqHnrpfmnLrjgILigJzlhq/Ct+ivuuS+neabvOe7k+aehOKAneacieS4pOS4quWFs+mUrueCue+8jOS4gOaYr+aMh+WHuuimgeWwhuWtmOWCqOiuvuWkh+S4juS4reWkruWkhOeQhuWZqOWIhuW8gO+8jOS6jOaYr+aPkOWHuuS6huWwhuaVsOaNruS7peS6jOi/m+WItuaWueW8j+e8lueggeOAguS6jOi/m+WItuaYr+S4gOenjeKAnOmAouS6jOi/m+S4gOKAneeahOiuoeaVsOazle+8jOi3n+aIkeS7rOS6uuexu+S9v+eUqOeahOKAnOmAouWNgei/m+S4gOKAneeahOiuoeaVsOazleayoeacieWunui0qOaAp+eahOWMuuWIq++8jOS6uuexu+WboOS4uuacieWNgeagueaJi+aMh+aJgOS7peS9v+eUqOS6huWNgei/m+WItu+8iOWboOS4uuWcqOaVsOaVsOaXtuWNgeagueaJi+aMh+eUqOWujOS5i+WQjuWwseWPquiDvei/m+S9jeS6hu+8jOW9k+eEtuWHoeS6i+mDveacieS+i+Wklu+8jOeOm+mbheS6uuWPr+iDveaYr+WboOS4uumVv+W5tOWFieedgOiEmueahOWOn+WboOaKiuiEmui2vuWktOS5n+eul+S4iuS6hu+8jOS6juaYr+S7luS7rOS9v+eUqOS6huS6jOWNgei/m+WItueahOiuoeaVsOazle+8jOWcqOi/meenjeiuoeaVsOazleeahOaMh+WvvOS4i+eOm+mbheS6uueahOWOhuazleWwseS4juaIkeS7rOW5s+W4uOS9v+eUqOeahOWOhuazleS4jeS4gOagt++8jOiAjOaMieeFp+eOm+mbheS6uueahOWOhuazle+8jDIwMTLlubTmmK/kuIrkuIDkuKrmiYDosJPnmoTigJzlpKrpmLPnuqrigJ3nmoTmnIDlkI7kuIDlubTvvIzogIwyMDEz5bm05YiZ5piv5paw55qE4oCc5aSq6Ziz57qq4oCd55qE5byA5aeL77yM5ZCO5p2l6L+Z5Lu25LqL5oOF6KKr5Lul6K655Lyg6K6555qE5pa55byP6K+v5Lyg5Li64oCdMjAxMuW5tOaYr+eOm+mbheS6uumihOiogOeahOS4lueVjOacq+aXpeKAnOi/meenjeiNkuivnueahOivtOazle+8jOS7iuWkqeaIkeS7rOWPr+S7peWkp+iDhueahOeMnOa1i++8jOeOm+mbheaWh+aYjuS5i+aJgOS7peWPkeWxlee8k+aFouS8sOiuoeS5n+S4juS9v+eUqOS6huS6jOWNgei/m+WItuacieWFs++8ieOAguWvueS6juiuoeeul+acuuadpeivtO+8jOS6jOi/m+WItuWcqOeJqeeQhuWZqOS7tuS4iuadpeivtOaYr+acgOWuueaYk+WunueOsOeahO+8iOmrmOeUteWOi+ihqOekujHvvIzkvY7nlLXljovooajnpLow77yJ77yM5LqO5piv5Zyo4oCc5Yavwrfor7rkvp3mm7znu5PmnoTigJ3nmoTorqHnrpfmnLrpg73kvb/nlKjkuobkuozov5vliLbjgILomb3nhLbmiJHku6zlubbkuI3pnIDopoHmr4/kuKrnqIvluo/lkZjpg73og73lpJ/kvb/nlKjkuozov5vliLbnmoTmgJ3nu7TmlrnlvI/mnaXlt6XkvZzvvIzkvYbmmK/kuobop6Pkuozov5vliLbku6Xlj4rlroPkuI7miJHku6znlJ/mtLvkuK3nmoTljYHov5vliLbkuYvpl7TnmoTovazmjaLlhbPns7vvvIzku6Xlj4rkuozov5vliLbkuI7lhavov5vliLblkozljYHlha3ov5vliLbnmoTovazmjaLlhbPns7vov5jmmK/mnInlv4XopoHnmoTjgILlpoLmnpzkvaDlr7nov5nkuIDngrnkuI3nhp/mgonvvIzlj6/ku6Xoh6rooYzkvb/nlKhb57u05Z+655m+56eRXShodHRwczovL3poLndpa2lwZWRpYS5vcmcvd2lraS8lRTQlQkElOEMlRTglQkYlOUIlRTUlODglQjYp5oiW6ICFW+eZvuW6pueZvuenkV0oaHR0cHM6Ly9iYWlrZS5iYWlkdS5jb20p56eR5pmu5LiA5LiL44CCCgojIyMg5Y+Y6YeP5ZKM57G75Z6LCgrlnKjnqIvluo/orr7orqHkuK3vvIzlj5jph4/mmK/kuIDnp43lrZjlgqjmlbDmja7nmoTovb3kvZPjgILorqHnrpfmnLrkuK3nmoTlj5jph4/mmK/lrp7pmYXlrZjlnKjnmoTmlbDmja7miJbogIXor7TmmK/lrZjlgqjlmajkuK3lrZjlgqjmlbDmja7nmoTkuIDlnZflhoXlrZjnqbrpl7TvvIzlj5jph4/nmoTlgLzlj6/ku6Xooqvor7vlj5blkozkv67mlLnvvIzov5nmmK/miYDmnInorqHnrpflkozmjqfliLbnmoTln7rnoYDjgILorqHnrpfmnLrog73lpITnkIbnmoTmlbDmja7mnInlvojlpJrnp43nsbvlnovvvIzpmaTkuobmlbDlgLzkuYvlpJbov5jlj6/ku6XlpITnkIbmlofmnKzjgIHlm77lvaLjgIHpn7PpopHjgIHop4bpopHnrYnlkITnp43lkITmoLfnmoTmlbDmja7vvIzpgqPkuYjkuI3lkIznmoTmlbDmja7lsLHpnIDopoHlrprkuYnkuI3lkIznmoTlrZjlgqjnsbvlnovjgIJQeXRob27kuK3nmoTmlbDmja7nsbvlnovlvojlpJrvvIzogIzkuJTkuZ/lhYHorrjmiJHku6zoh6rlrprkuYnmlrDnmoTmlbDmja7nsbvlnovvvIjov5nkuIDngrnlnKjlkI7pnaLkvJrorrLliLDvvInvvIzmiJHku6zlhYjku4vnu43lh6Dnp43luLjnlKjnmoTmlbDmja7nsbvlnovjgIIKCi0g5pW05Z6L77yaUHl0aG9u5Lit5Y+v5Lul5aSE55CG5Lu75oSP5aSn5bCP55qE5pW05pWw77yIUHl0aG9uIDIueOS4reaciWludOWSjGxvbmfkuKTnp43nsbvlnovnmoTmlbTmlbDvvIzkvYbov5nnp43ljLrliIblr7lQeXRob27mnaXor7TmhI/kuYnkuI3lpKfvvIzlm6DmraTlnKhQeXRob24gMy545Lit5pW05pWw5Y+q5pyJaW506L+Z5LiA56eN5LqG77yJ77yM6ICM5LiU5pSv5oyB5LqM6L+b5Yi277yI5aaCYDBiMTAwYO+8jOaNoueul+aIkOWNgei/m+WItuaYrzTvvInjgIHlhavov5vliLbvvIjlpoJgMG8xMDBg77yM5o2i566X5oiQ5Y2B6L+b5Yi25pivNjTvvInjgIHljYHov5vliLbvvIhgMTAwYO+8ieWSjOWNgeWFrei/m+WItu+8iGAweDEwMGDvvIzmjaLnrpfmiJDljYHov5vliLbmmK8yNTbvvInnmoTooajnpLrms5XjgIIKLSDmta7ngrnlnovvvJrmta7ngrnmlbDkuZ/lsLHmmK/lsI/mlbDvvIzkuYvmiYDku6Xnp7DkuLrmta7ngrnmlbDvvIzmmK/lm6DkuLrmjInnhafnp5HlraborrDmlbDms5XooajnpLrml7bvvIzkuIDkuKrmta7ngrnmlbDnmoTlsI/mlbDngrnkvY3nva7mmK/lj6/lj5jnmoTvvIzmta7ngrnmlbDpmaTkuobmlbDlrablhpnms5XvvIjlpoJgMTIzLjQ1NmDvvInkuYvlpJbov5jmlK/mjIHnp5HlraborqHmlbDms5XvvIjlpoJgMS4yMzQ1NmUyYO+8ieOAggotIOWtl+espuS4suWei++8muWtl+espuS4suaYr+S7peWNleW8leWPt+aIluWPjOW8leWPt+aLrOi1t+adpeeahOS7u+aEj+aWh+acrO+8jOavlOWmgmAnaGVsbG8nYOWSjGAiaGVsbG8iYCzlrZfnrKbkuLLov5jmnInljp/lp4vlrZfnrKbkuLLooajnpLrms5XjgIHlrZfoioLlrZfnrKbkuLLooajnpLrms5XjgIFVbmljb2Rl5a2X56ym5Liy6KGo56S65rOV77yM6ICM5LiU5Y+v5Lul5Lmm5YaZ5oiQ5aSa6KGM55qE5b2i5byP77yI55So5LiJ5Liq5Y2V5byV5Y+35oiW5LiJ5Liq5Y+M5byV5Y+35byA5aS077yM5LiJ5Liq5Y2V5byV5Y+35oiW5LiJ5Liq5Y+M5byV5Y+357uT5bC+77yJ44CCCi0g5biD5bCU5Z6L77ya5biD5bCU5YC85Y+q5pyJYFRydWVg44CBYEZhbHNlYOS4pOenjeWAvO+8jOimgeS5iOaYr2BUcnVlYO+8jOimgeS5iOaYr2BGYWxzZWDvvIzlnKhQeXRob27kuK3vvIzlj6/ku6Xnm7TmjqXnlKhgVHJ1ZWDjgIFgRmFsc2Vg6KGo56S65biD5bCU5YC877yI6K+35rOo5oSP5aSn5bCP5YaZ77yJ77yM5Lmf5Y+v5Lul6YCa6L+H5biD5bCU6L+Q566X6K6h566X5Ye65p2l77yI5L6L5aaCYDMgPCA1YOS8muS6p+eUn+W4g+WwlOWAvGBUcnVlYO+8jOiAjGAyID09IDFg5Lya5Lqn55Sf5biD5bCU5YC8YEZhbHNlYO+8ieOAggotIOWkjeaVsOWei++8muW9ouWmgmAzKzVqYO+8jOi3n+aVsOWtpuS4iueahOWkjeaVsOihqOekuuS4gOagt++8jOWUr+S4gOS4jeWQjOeahOaYr+iZmumDqOeahGBpYOaNouaIkOS6hmBqYOOAgg=="),
            ("Day03 - 分支结构","IyMgRGF5MDMgLSDliIbmlK/nu5PmnoQKCiMjIyDliIbmlK/nu5PmnoTnmoTlupTnlKjlnLrmma8KCui/hOS7iuS4uuatou+8jOaIkeS7rOWGmeeahFB5dGhvbuS7o+eggemDveaYr+S4gOadoeS4gOadoeivreWPpemhuuW6j+aJp+ihjO+8jOi/meenjee7k+aehOeahOS7o+eggeaIkeS7rOensOS5i+S4uumhuuW6j+e7k+aehOOAgueEtuiAjOS7heaciemhuuW6j+e7k+aehOW5tuS4jeiDveino+WGs+aJgOacieeahOmXrumimO+8jOavlOWmguaIkeS7rOiuvuiuoeS4gOS4qua4uOaIj++8jOa4uOaIj+esrOS4gOWFs+eahOmAmuWFs+adoeS7tuaYr+eOqeWutuiOt+W+lzEwMDDliIbvvIzpgqPkuYjlnKjlrozmiJDmnKzlsYDmuLjmiI/lkI7miJHku6zopoHmoLnmja7njqnlrrblvpfliLDliIbmlbDmnaXlhrPlrprnqbbnq5/mmK/ov5vlhaXnrKzkuozlhbPov5jmmK/lkYror4nnjqnlrrbigJxHYW1lIE92ZXLigJ3vvIzov5nph4zlsLHkvJrkuqfnlJ/kuKTkuKrliIbmlK/vvIzogIzkuJTov5nkuKTkuKrliIbmlK/lj6rmnInkuIDkuKrkvJrooqvmiafooYzvvIzov5nlsLHmmK/nqIvluo/kuK3liIbmlK/nu5PmnoTjgILnsbvkvLznmoTlnLrmma/ov5jmnInlvojlpJrvvIznu5nlpKflrrbkuIDliIbpkp/nmoTml7bpl7TvvIzkvaDlupTor6Xlj6/ku6Xmg7PliLDoh7PlsJE15Liq5Lul5LiK6L+Z5qC355qE5L6L5a2Q77yM6LW257Sn6K+V5LiA6K+V44CCCgojIyMgaWbor63lj6XnmoTkvb/nlKgKCuWcqFB5dGhvbuS4re+8jOimgeaehOmAoOWIhuaUr+e7k+aehOWPr+S7peS9v+eUqGBpZmDjgIFgZWxpZmDlkoxgZWxzZWDlhbPplK7lrZfjgILmiYDosJPlhbPplK7lrZflsLHmmK/mnInnibnmrorlkKvkuYnnmoTljZXor43vvIzlg49gaWZg5ZKMYGVsc2Vg5bCx5piv5LiT6Zeo55So5LqO5p6E6YCg5YiG5pSv57uT5p6E55qE5YWz6ZSu5a2X77yM5b6I5pi+54S25L2g5LiN6IO95aSf5L2/55So5a6D5L2c5Li65Y+Y6YeP5ZCN77yI5LqL5a6e5LiK77yM55So5L2c5YW25LuW55qE5qCH6K+G56ym5Lmf5piv5LiN5Y+v5Lul77yJ44CC5LiL6Z2i55qE5L6L5a2Q5Lit5ryU56S65LqG5aaC5L2V5p6E6YCg5LiA5Liq5YiG5pSv57uT5p6E44CCCgpgYGBQeXRob24KIiIiCueUqOaIt+i6q+S7vemqjOivgQoKVmVyc2lvbjogMC4xCkF1dGhvcjog6aqG5piKCiIiIgoKdXNlcm5hbWUgPSBpbnB1dCgn6K+36L6T5YWl55So5oi35ZCNOiAnKQpwYXNzd29yZCA9IGlucHV0KCfor7fovpPlhaXlj6Pku6Q6ICcpCiMg5aaC5p6c5biM5pyb6L6T5YWl5Y+j5Luk5pe2IOe7iOerr+S4reayoeacieWbnuaYviDlj6/ku6Xkvb/nlKhnZXRwYXNz5qih5Z2X55qEZ2V0cGFzc+WHveaVsAojIGltcG9ydCBnZXRwYXNzCiMgcGFzc3dvcmQgPSBnZXRwYXNzLmdldHBhc3MoJ+ivt+i+k+WFpeWPo+S7pDogJykKaWYgdXNlcm5hbWUgPT0gJ2FkbWluJyBhbmQgcGFzc3dvcmQgPT0gJzEyMzQ1Nic6CiAgICBwcmludCgn6Lqr5Lu96aqM6K+B5oiQ5YqfIScpCmVsc2U6CiAgICBwcmludCgn6Lqr5Lu96aqM6K+B5aSx6LSlIScpCmBgYAoK5ZSv5LiA6ZyA6KaB6K+05piO55qE5piv5ZKMQy9DKyvjgIFKYXZh562J6K+t6KiA5LiN5ZCM77yMUHl0aG9u5Lit5rKh5pyJ55So6Iqx5ous5Y+35p2l5p6E6YCg5Luj56CB5Z2X6ICM5piv5L2/55So5LqG57yp6L+b55qE5pa55byP5p2l6K6+572u5Luj56CB55qE5bGC5qyh57uT5p6E77yM5aaC5p6cYGlmYOadoeS7tuaIkOeri+eahOaDheWGteS4i+mcgOimgeaJp+ihjOWkmuadoeivreWPpe+8jOWPquimgeS/neaMgeWkmuadoeivreWPpeWFt+acieebuOWQjOeahOe8qei/m+WwseWPr+S7peS6hu+8jOaNouWPpeivneivtOi/nue7reeahOS7o+eggeWmguaenOWPiOS/neaMgeS6huebuOWQjOeahOe8qei/m+mCo+S5iOWug+S7rOWxnuS6juWQjOS4gOS4quS7o+eggeWdl++8jOebuOW9k+S6juaYr+S4gOS4quaJp+ihjOeahOaVtOS9k+OAggoK5b2T54S25aaC5p6c6KaB5p6E6YCg5Ye65pu05aSa55qE5YiG5pSv77yM5Y+v5Lul5L2/55SoYGlm4oCmZWxpZuKApmVsc2XigKZg57uT5p6E77yM5L6L5aaC5LiL6Z2i55qE5YiG5q615Ye95pWw5rGC5YC844CCCgohWyQkZih4KT1cYmVnaW57Y2FzZXN9IDN4LTUmXHRleHR7KHg+MSl9XFx4KzImXHRleHR7KC0xfVxsZXFcdGV4dHt4fVxsZXFcdGV4dHsxKX1cXDV4KzMmXHRleHQgeyh4PC0xKX1cZW5ke2Nhc2VzfSQkXSguL3Jlcy9mb3JtdWxhXzEucG5nKQoKYGBgUHl0aG9uCiIiIgrliIbmrrXlh73mlbDmsYLlgLwKCiAgICAgICAgM3ggLSA1ICAoeCA+IDEpCmYoeCkgPSAgeCArIDIgICAoLTEgPD0geCA8PSAxKQogICAgICAgIDV4ICsgMyAgKHggPCAtMSkKClZlcnNpb246IDAuMQpBdXRob3I6IOmqhuaYigoiIiIKCnggPSBmbG9hdChpbnB1dCgneCA9ICcpKQppZiB4ID4gMToKICAgIHkgPSAzICogeCAtIDUKZWxpZiB4ID49IC0xOgogICAgeSA9IHggKyAyCmVsc2U6CiAgICB5ID0gNSAqIHggKyAzCnByaW50KCdmKCUuMmYpID0gJS4yZicgJSAoeCwgeSkpCmBgYA==")};
    }
}

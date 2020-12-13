using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.AssetDB.SeedData
{
    public static class AssetDbSeedData
    {
        private static List<App> AppData
        {
            get
            {
                return new List<App>
                {
                    new App { Id = 1, Name = "Smart Lock" },
                    new App { Id = 2, Name = "Remote Monitoring System (RMS)" }
                };
            }
        }
        private static List<MobileOperator> Operators
        {
            get
            {
                return new List<MobileOperator>
                {
                    new MobileOperator { Id = 1, Name = "Robi" },
                    new MobileOperator { Id = 2, Name = "Banglalink" },
                    new MobileOperator { Id = 3, Name = "Grameenphone" },
                    new MobileOperator { Id= 4, Name = "Airtel" },
                    new MobileOperator { Id = 5, Name = "Teletalk" }
                };
            }
        }
        public static void InsertUpdateSeedData(string connectionString)
        {
            using (var context = new EyeAssetDbContext(connectionString))
            {
                context.Database.EnsureCreated();
                var apps = context.Apps.ToList();
                if (apps == null)
                {
                    context.Apps.AddRange(AppData);
                }
                else
                {
                    for (int i = 0; i < AppData.Count; i++)
                    {
                        var existing = apps.FirstOrDefault(x => x.Id == AppData[i].Id);
                        if (existing == null)
                        {
                            context.Apps.Add(AppData[i]);
                        }
                        else
                        {
                            if (existing.Name != AppData[i].Name)
                            {
                                existing.Name = AppData[i].Name;
                                context.Entry(existing).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            }
                        }
                    }
                }
                var operators = context.MobileOperators.ToList();
                if (operators == null)
                {
                    context.MobileOperators.AddRange(Operators);
                }
                else
                {
                    for (int i = 0; i < Operators.Count; i++)
                    {
                        var existing = operators.FirstOrDefault(x => x.Id == Operators[i].Id);
                        if (existing == null)
                        {
                            context.MobileOperators.Add(Operators[i]);
                        }
                        else
                        {
                            if (existing.Name != Operators[i].Name)
                            {
                                existing.Name = Operators[i].Name;
                                context.Entry(existing).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            }
                        }
                    }
                }
                context.SaveChanges();
            }
        }
    }
}

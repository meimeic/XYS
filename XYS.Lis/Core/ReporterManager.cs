using System;
using System.Reflection;
using XYS.Lis.Repository;
using XYS.Lis.Util;

namespace XYS.Lis.Core
{
   public class ReporterManager
    {
       private static IRepositorySelector s_repositorySelector;
       private static readonly Type declaringType=typeof(ReporterManager);
       #region
       private ReporterManager() 
		{
		}
       static ReporterManager()
       {
           string appRepositorySelectorTypeName = SystemInfo.GetAppSetting("lis-report.RepositorySelector");
           if (appRepositorySelectorTypeName != null && appRepositorySelectorTypeName.Length > 0)
           {
               // Resolve the config string into a Type
               Type appRepositorySelectorType = null;
               try
               {
                   appRepositorySelectorType = SystemInfo.GetTypeFromString(appRepositorySelectorTypeName, false, true);
               }
               catch (Exception ex)
               {
                   ReportReport.Error(declaringType, "Exception while resolving RepositorySelector Type [" + appRepositorySelectorTypeName + "]", ex);
               }

               if (appRepositorySelectorType != null)
               {
                   // Create an instance of the RepositorySelectorType
                   object appRepositorySelectorObj = null;
                   try
                   {
                       appRepositorySelectorObj = Activator.CreateInstance(appRepositorySelectorType);
                   }
                   catch (Exception ex)
                   {
                       ReportReport.Error(declaringType, "Exception while creating RepositorySelector [" + appRepositorySelectorType.FullName + "]", ex);
                   }

                   if (appRepositorySelectorObj != null && appRepositorySelectorObj is IRepositorySelector)
                   {
                       s_repositorySelector = (IRepositorySelector)appRepositorySelectorObj;
                   }
                   else
                   {
                       ReportReport.Error(declaringType, "RepositorySelector Type [" + appRepositorySelectorType.FullName + "] is not an IRepositorySelector");
                   }
               }
           }
           if (s_repositorySelector == null)
           {
               s_repositorySelector = new DefaultRepositorySelector(typeof(XYS.Lis.Repository.Hierarchy.Hierarchy));
           }
       }
       #endregion

       public static IRepositorySelector RepositorySelector
       {
           get { return s_repositorySelector; }
           set { s_repositorySelector = value; }
       }
       
       #region
       public static IReporterRepository GetRepository(string repository)
       {
           if (repository == null)
           {
               throw new ArgumentNullException("repository");
           }
           return RepositorySelector.GetRepository(repository);
       }
       public static IReporterRepository GetRepository(Assembly repositoryAssembly)
       {
           if (repositoryAssembly == null)
           {
               throw new ArgumentNullException("repositoryAssembly");
           }
           return RepositorySelector.GetRepository(repositoryAssembly);
       }
       #endregion

       #region
       public static ILisReporter Exists(string repository, ReporterKey key)
       {
           if (repository == null)
           {
               throw new ArgumentNullException("repository");
           }
           if (key == null)
           {
               throw new ArgumentNullException("name");
           }
           return RepositorySelector.GetRepository(repository).Exists(key);
       }
       public static ILisReporter Exists(Assembly repositoryAssembly, ReporterKey key)
       {
           if (repositoryAssembly == null)
           {
               throw new ArgumentNullException("repositoryAssembly");
           }
           if (key == null)
           {
               throw new ArgumentNullException("ReporterKey");
           }
           return RepositorySelector.GetRepository(repositoryAssembly).Exists(key);
       }
       #endregion

       #region
       public static ILisReporter GetReporter(string repository, ReporterKey key)
       {
           if (repository == null)
           {
               throw new ArgumentNullException("repository");
           }
           if (key == null)
           {
               throw new ArgumentNullException("ReporterKey");
           }
           return RepositorySelector.GetRepository(repository).GetReporter(key);
       }
       public static ILisReporter GetReporter(string repository, Type type)
       {
           if (repository == null)
           {
               throw new ArgumentNullException("repository");
           }
           if (type == null)
           {
               throw new ArgumentNullException("type");
           }
           ReporterKey key = new ReporterKey(type.FullName);
           return RepositorySelector.GetRepository(repository).GetReporter(key);
       }
       public static ILisReporter GetReporter(string repository, Type type, string strategyName)
       {
           if (repository == null)
           {
               throw new ArgumentNullException("repository");
           }
           if (type == null)
           {
               throw new ArgumentNullException("type");
           }
           ReporterKey key = new ReporterKey(type.FullName, strategyName);
           return RepositorySelector.GetRepository(repository).GetReporter(key);
       }
       public static ILisReporter GetReporter(Assembly repositoryAssembly, ReporterKey key)
       {
           if (repositoryAssembly == null)
           {
               throw new ArgumentNullException("repositoryAssembly");
           }
           if (key == null)
           {
               throw new ArgumentNullException("ReporterKey");
           }
           return RepositorySelector.GetRepository(repositoryAssembly).GetReporter(key);
       }
       public static ILisReporter GetReporter(Assembly repositoryAssembly, Type type)
       {
           if (repositoryAssembly == null)
           {
               throw new ArgumentNullException("repositoryAssembly");
           }
           if (type == null)
           {
               throw new ArgumentNullException("type");
           }
           ReporterKey key = new ReporterKey(type.FullName);
           return RepositorySelector.GetRepository(repositoryAssembly).GetReporter(key);
       }
       public static ILisReporter GetReporter(Assembly repositoryAssembly, Type type, string strategyName)
       {
           if (repositoryAssembly == null)
           {
               throw new ArgumentNullException("repositoryAssembly");
           }
           if (type == null)
           {
               throw new ArgumentNullException("type");
           }
           ReporterKey key = new ReporterKey(type.FullName, strategyName);
           return RepositorySelector.GetRepository(repositoryAssembly).GetReporter(key);
       }
       #endregion

       #region
       public static IReporterRepository CreateRepository(string repository)
       {
           if (repository == null)
           {
               throw new ArgumentNullException("repository");
           }
           return RepositorySelector.CreateRepository(repository, null);
       }
       public static IReporterRepository CreateRepository(string repository, Type repositoryType)
       {
           if (repository == null)
           {
               throw new ArgumentNullException("repository");
           }
           if (repositoryType == null)
           {
               throw new ArgumentNullException("repositoryType");
           }
           return RepositorySelector.CreateRepository(repository, repositoryType);
       }
       public static IReporterRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType)
       {
           if (repositoryAssembly == null)
           {
               throw new ArgumentNullException("repositoryAssembly");
           }
           if (repositoryType == null)
           {
               throw new ArgumentNullException("repositoryType");
           }
           return RepositorySelector.CreateRepository(repositoryAssembly, repositoryType);
       }
       #endregion

       public static IReporterRepository[] GetAllRepositories()
       {
           return RepositorySelector.GetAllRepositories();
       }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPLOSION_HUB.Server.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		int Save();
	}
}

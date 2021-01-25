using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LenelServices.Repositories.Interfaces;
using DataConduitManager.Repositories.DTO;
using DataConduitManager.Repositories.Interfaces;

namespace LenelServices.Repositories.Logic
{
    public class Badge_REP_LOCAL : IBadge_REP_LOCAL
    {
        private readonly IBadge _badge_REP;

        public Badge_REP_LOCAL(IBadge badge_REP)
        {
            _badge_REP = badge_REP;
        }

        public async Task<object> CrearBadge(AddBadge_DTO newBadge)
        {
            return await _badge_REP.AddBadge(newBadge);
        }
    }
}

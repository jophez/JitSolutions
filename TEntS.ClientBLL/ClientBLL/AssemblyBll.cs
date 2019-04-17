using System;
using System.Collections.Generic;
using TEntS.ClientBLL.Interfaces;
using TEntS.Types;
using TEntS.Types.Assembly;

namespace TEntS.ClientBLL.ClientBLL
					{
					public class AssemblyBll : IAssemblyBll
        {
        // private WynPowerDbEntities context;
        IAssemblyBll _base;
        public AssemblyBll(IAssemblyBll assemblyBll)
            {
            _base = assemblyBll;
            //   context = new WynPowerDbEntities();
            }

        public bool Activate(int assemblyId, UserDetails userDetails)
            {
            //var isSuccessful = false;
            //try
            //{
            //    var result = context.ActivateAssembly(assemblyId, userDetails.Id.ToString()).ToString();
            //    isSuccessful = int.Parse(result) > 0 ? true : false;
            //}
            //catch (Exception ex)
            //{
            //    throw new ApplicationException(ex.Message);
            //}
            return true;
            }

        public bool Create(Assembly assemblyObj, UserDetails userDetails)
            {
            var isSuccessful = false;
            try
                {
                isSuccessful = _base.Create(assemblyObj, userDetails);
                // isSuccessful = int.Parse(result) > 0 ? true : false;
                }
            catch (Exception ex)
                {
                throw new ApplicationException(ex.Message);
                }
            return isSuccessful;
            }

        public bool Retire(int assemblyId, UserDetails userDetails)
            {
            var isSuccessful = false;
            try
                {
                isSuccessful = _base.Retire(assemblyId, userDetails);
                }
            catch (Exception ex)
                {
                throw new ApplicationException(ex.Message);
                }
            return isSuccessful;
            }

        public List<Assembly> RetrieveActiveAssemblyDetails()
            {
            try
                {
                return _base.RetrieveActiveAssemblyDetails();
                }
            catch (Exception ex)
                {
                throw new TEntS.Types.Exception.TEntSException(ex.Message);
                }
            }

        public List<Assembly> RetrieveAssemblyDetails()
            {

            try
                {
                return _base.RetrieveAssemblyDetails();
                }
            catch (Exception ex)
                {
                throw new TEntS.Types.Exception.TEntSException(ex.Message);
                }
            }

        public Assembly RetrieveAssemblyDetailsById(int assemblyId)
            {
            try
                {
                return _base.RetrieveAssemblyDetailsById(assemblyId);
                }
            catch (Exception ex)
                {
                throw new TEntS.Types.Exception.TEntSException(ex.Message);
                }
            }

        public bool Update(Assembly assemblyObj, UserDetails userDetails)
            {
            var isSuccessful = false;
            try
                {
                isSuccessful = _base.Update(assemblyObj, userDetails);
                // isSuccessful = int.Parse(result) > 0 ? true : false;
                }
            catch (Exception ex)
                {
                throw new ApplicationException(ex.Message);
                }
            return isSuccessful;
            }

        public List<Types.Materials.MaterialForAssembly> RetrieveAssemblyMaterialsByCode(string assemblyCode)
            {
            try
                {
                return _base.RetrieveAssemblyMaterialsByCode(assemblyCode);
                }
            catch (Exception ex)
                {
                throw new TEntS.Types.Exception.TEntSException(ex.Message);
                }
            }

        public List<Types.Materials.MaterialForAssembly> RetrieveAssemblyMaterialsByClassification(string classification)
            {
            try
                {
                return _base.RetrieveAssemblyMaterialsByClassification(classification);
                }
            catch (Exception ex)
                {
                throw new TEntS.Types.Exception.TEntSException(ex.Message);
                }
            }
        }
    }


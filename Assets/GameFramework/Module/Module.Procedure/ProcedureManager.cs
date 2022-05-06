﻿using System;
using GameFramework.Module.Fsm;

namespace GameFramework.Module.Procedure
{
    /// <summary>
    /// 流程管理器。
    /// </summary>
    internal sealed class ProcedureManager : IModule, IProcedureManager
    {
        private IFsmManager _fsmManager;
        private IFsm<IProcedureManager> _procedureFsm;
        private static ProcedureManager _instance;

        /// <summary>
        /// 初始化流程管理器的新实例。
        /// </summary>
        public ProcedureManager()
        {
            _fsmManager = null;
            _procedureFsm = null;
        }

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        public int Priority => 0;
        
        public static ProcedureManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    ModuleManager.GetModule<ProcedureManager>();
                }

                return _instance;
            }
        }

        /// <summary>
        /// 获取当前流程。
        /// </summary>
        public ProcedureBase CurrentProcedure
        {
            get
            {
                if (_procedureFsm == null)
                {
                    throw new Exception("You must initialize procedure first.");
                }

                return (ProcedureBase)_procedureFsm.CurrentState;
            }
        }

        /// <summary>
        /// 获取当前流程持续时间。
        /// </summary>
        public float CurrentProcedureTime
        {
            get
            {
                if (_procedureFsm == null)
                {
                    throw new Exception("You must initialize procedure first.");
                }

                return _procedureFsm.CurrentStateTime;
            }
        }

        public void OnCreate()
        {
            _instance = this;
        }

        /// <summary>
        /// 流程管理器轮询。
        /// </summary>
        public void Update()
        {
        }

        /// <summary>
        /// 关闭并清理流程管理器。
        /// </summary>
        public void Shutdown()
        {
            if (_fsmManager != null)
            {
                if (_procedureFsm != null)
                {
                    _fsmManager.DestroyFsm(_procedureFsm);
                    _procedureFsm = null;
                }

                _fsmManager = null;
            }
        }

        /// <summary>
        /// 初始化流程管理器。
        /// </summary>
        /// <param name="fsmManager">有限状态机管理器。</param>
        /// <param name="procedures">流程管理器包含的流程。</param>
        public void Initialize(IFsmManager fsmManager, params ProcedureBase[] procedures)
        {
            if (fsmManager == null)
            {
                throw new Exception("FSM manager is invalid.");
            }

            _fsmManager = fsmManager;
            _procedureFsm = _fsmManager.CreateFsm(this, procedures);
        }

        /// <summary>
        /// 开始流程。
        /// </summary>
        /// <typeparam name="T">要开始的流程类型。</typeparam>
        public void StartProcedure<T>() where T : ProcedureBase
        {
            if (_procedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            _procedureFsm.Start<T>();
        }

        /// <summary>
        /// 开始流程。
        /// </summary>
        /// <param name="procedureType">要开始的流程类型。</param>
        public void StartProcedure(Type procedureType)
        {
            if (_procedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            _procedureFsm.Start(procedureType);
        }

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <typeparam name="T">要检查的流程类型。</typeparam>
        /// <returns>是否存在流程。</returns>
        public bool HasProcedure<T>() where T : ProcedureBase
        {
            if (_procedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return _procedureFsm.HasState<T>();
        }

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <param name="procedureType">要检查的流程类型。</param>
        /// <returns>是否存在流程。</returns>
        public bool HasProcedure(Type procedureType)
        {
            if (_procedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return _procedureFsm.HasState(procedureType);
        }

        /// <summary>
        /// 获取流程。
        /// </summary>
        /// <typeparam name="T">要获取的流程类型。</typeparam>
        /// <returns>要获取的流程。</returns>
        public ProcedureBase GetProcedure<T>() where T : ProcedureBase
        {
            if (_procedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return _procedureFsm.GetState<T>();
        }

        /// <summary>
        /// 获取流程。
        /// </summary>
        /// <param name="procedureType">要获取的流程类型。</param>
        /// <returns>要获取的流程。</returns>
        public ProcedureBase GetProcedure(Type procedureType)
        {
            if (_procedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return (ProcedureBase)_procedureFsm.GetState(procedureType);
        }
    }
}
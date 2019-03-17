
namespace Auction.Entities.Enums
{
    /// <summary>
    /// 通用枚举类
    /// </summary>
    public class CommonEnum
    {
        /// <summary>
        /// 是否已删
        /// </summary>
        public enum IsDeleted
        {
            /// <summary>
            /// 所有 -1
            /// </summary>
            All = -1,
            /// <summary>
            /// 否 0
            /// </summary>
            No = 0,
            /// <summary>
            /// 是 1
            /// </summary>
            Yes = 1
        }

        /// <summary>
        /// 是否已被锁定
        /// </summary>
        public enum IsLocked
        {
            /// <summary>
            /// 未锁定 0
            /// </summary>
            UnLocked = 0,
            /// <summary>
            /// 已锁定 1
            /// </summary>
            Locked = 1
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        public enum IsEnabled
        {
            /// <summary>
            /// 否 0
            /// </summary>
            No = 0,
            /// <summary>
            /// 是 1
            /// </summary>
            Yes = 1
        }


        /// <summary>
        /// 用户状态
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// 未指定 -1
            /// </summary>
            All = -1,
            /// <summary>
            /// 已禁用 0
            /// </summary>
            Forbidden = 0,
            /// <summary>
            /// 正常 1
            /// </summary>
            Normal = 1
        }

        /// <summary>
        /// 权限类型
        /// </summary>
        public enum PermissionType
        {
            /// <summary>
            /// 菜单 0
            /// </summary>
            Menu = 0,
            /// <summary>
            /// 按钮/操作/功能 1
            /// </summary>
            Action = 1
        }

        /// <summary>
        /// 是否枚举
        /// </summary>
        public enum YesOrNo
        {
            /// <summary>
            /// 所有 -1
            /// </summary>
            All = -1,
            /// <summary>
            /// 否 0
            /// </summary>
            No = 0,
            /// <summary>
            /// 是 1
            /// </summary>
            Yes = 1
        }


        /// <summary>
        /// 用户角色
        /// </summary>
        public enum UserRole
        {
            /// <summary>
            /// 管理员 -1
            /// </summary>
            Admin = -1,
            /// <summary>
            /// 员工 9
            /// </summary>
            Staff = 9,
            /// <summary>
            /// 客人 0
            /// </summary>
            Guest = 0,
            /// <summary>
            /// 会员 5
            /// </summary>
            Member = 5
        }

        /// <summary>
        /// 用户状态
        /// </summary>
        public enum UserStatus
        {
            /// <summary>
            /// 未指定 -1
            /// </summary>
            All = -1,
            /// <summary>
            /// 已禁用 0
            /// </summary>
            Forbidden = 0,
            /// <summary>
            /// 正常 1
            /// </summary>
            Normal = 1
        }
        /// <summary>
        /// 是否被拍卖
        /// </summary>
        public enum IsSold
        {
            /// <summary>
            /// 未指定 -1
            /// </summary>
            All = -1,
            /// <summary>
            /// 否 0
            /// </summary>
            No = 0,
            /// <summary>
            /// 是 1
            /// </summary>
            Yes = 1
        }

        /// <summary>
        /// 是否被拍卖
        /// </summary>
        public enum IsPurchase
        {
            /// <summary>
            /// 未指定 -1
            /// </summary>
            All = -1,
            /// <summary>
            /// 否 0
            /// </summary>
            No = 0,
            /// <summary>
            /// 是 1
            /// </summary>
            Yes = 1
        }

        public enum PhotoVersion
        {
            Normal = 48,
            Small = 16,
            large = 64,
            big = 120
        }


        /// <summary>
        /// 平台
        /// </summary>

        public enum Platfrom
        {
            /// <summary>
            /// WEB 1
            /// </summary>
            WEB = 1,
            /// <summary>
            /// Android 2
            /// </summary>
            Android = 2,
            /// <summary>
            /// IPhone 3
            /// </summary>
            IPhone = 3,
            /// <summary>
            /// WX 4
            /// </summary>
            WX = 4
        }
    }
}

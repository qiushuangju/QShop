using System;

namespace Qs.Repository.Domain
{
	/// <summary>
	/// 用户ID
	/// </summary>
	public static class UserExt
	{
	    public static void  CheckPassword(this ModelUser user, string password)
	    {
	        if (user.Password != password)
	        {
	            throw  new Exception("密码错误");
	        }
	    }

	}
} 
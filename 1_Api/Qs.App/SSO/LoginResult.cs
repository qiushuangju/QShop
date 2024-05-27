using Qs.Comm;    

namespace Qs.App.SSO
{
    /// <summary>
    /// ��¼
    /// </summary>
    public class LoginResult :Response<string>
    {
        /// <summary>
        /// �û�Id/IM�û�Id
        /// </summary>
        public string UserId;
        /// <summary>
        /// ע����ת·��
        /// </summary>
        public string ReturnUrl;
        /// <summary>
        /// Token
        /// </summary>
        public string Token;
        /// <summary>
        /// Ĭ���ǳ�
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// �û�����( 10:�û� 15:��Ա 20:˾�� 30:���� 40:ҵ��)
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        ///״̬ 10:���� -10:����
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// �Ƿ��Ѱ��ֻ�
        /// </summary>
        public bool IsBindPhone { get; set; }
        /// <summary>
        /// �Ƿ�������֧������
        /// </summary>
        public bool IsHavePayPwd { get; set; }

        /// <summary>
        /// Ĭ��ͷ��
        /// </summary>
        public string UrlAvatar { get; set; }

    }
}
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class UnityAuth : MonoBehaviour
    {
        private void OnEnable()
        {
            DontDestroyOnLoad(this);
            Login().Forget();
        }


        public async UniTask Login()
        {
            //Initialize the Unity Services engine
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                //If not already logged, log the user in
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
    }
}
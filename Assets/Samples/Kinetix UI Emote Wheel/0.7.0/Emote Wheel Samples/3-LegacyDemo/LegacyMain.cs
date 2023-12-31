using UnityEngine;
using Kinetix;
using Kinetix.UI;
using Kinetix.UI.EmoteWheel;

namespace Kinetix.Sample
{
    public class LegacyMain : MonoBehaviour
    {
        [SerializeField] private string virtualWorldKey;
        [SerializeField] private GameObject character;
        [SerializeField] private Avatar     avatar;
        [SerializeField] private Transform  rootTransform;

        private Animation animationComponent;

        private void Awake()
        {
            KinetixCore.OnInitialized += OnKinetixInitialized;
            KinetixCore.Initialize(new KinetixCoreConfiguration()
            {
                VirtualWorldKey = virtualWorldKey,
                PlayAutomaticallyAnimationOnAnimators = true,
                ShowLogs                              = true,
                EnableAnalytics                       = true
            });
        }

        private void OnDestroy()
        {
            KinetixCore.OnInitialized -= OnKinetixInitialized;
        }

        private void OnKinetixInitialized()
        {
            KinetixUIEmoteWheel.Initialize(new KinetixUIEmoteWheelConfiguration()
            {
                enabledCategories = new []
                {
                    EKinetixUICategory.INVENTORY,
                    EKinetixUICategory.EMOTE_SELECTOR
                }
            });

            // EVENTS UI
            KinetixUI.OnPlayedAnimationWithEmoteSelector += OnLocalPlayedAnimation;

            KinetixCore.Animation.RegisterLocalPlayerCustom(avatar, rootTransform, EExportType.AnimationClipLegacy);

            KinetixCore.Account.ConnectAccount("sdk-sample-user-id", OnAccountConnected);
        }

        private void OnAccountConnected()
        {
            KinetixCore.Account.AssociateEmotesToUser("d228a057-6409-4560-afd0-19c804b30b84");
            KinetixCore.Account.AssociateEmotesToUser("bd6749e5-ac29-46e4-aae2-bb1496d04cbb");
            KinetixCore.Account.AssociateEmotesToUser("7a6d483e-ebdc-4efd-badb-12a2e210e618");
        }

        private void OnLocalPlayedAnimation(AnimationIds _AnimationIds)
        {
            KinetixCore.Animation.GetRetargetedAnimationClipLegacyForLocalPlayer(_AnimationIds, (animationClip) =>
            {
                if (animationComponent == null)
                {
                    animationComponent                   = character.gameObject.AddComponent<Animation>();
                    animationComponent.playAutomatically = false;
                }

                animationComponent.AddClip(animationClip, _AnimationIds.UUID);
                animationComponent.Play(_AnimationIds.UUID);
            });
        }
    }
}

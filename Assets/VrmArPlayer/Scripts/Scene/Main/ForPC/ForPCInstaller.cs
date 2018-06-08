using VrmArPlaye;
using Zenject;

namespace VrmArPlayer
{
    public class ForPCInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //VRモード時はVRInputEventProviderを使う
            Container
                .Bind(typeof(IInputEventProvider), typeof(IInputSetable))
                .To<VrInputEventProvider>().AsCached();

            //ステージの状態は固定
            var scaler = gameObject.AddComponent<FixedStageScaler>();
            Container.Bind<IStageScaler>().FromInstance(scaler);

            var mainConfig = new Configs(isVrMode: true);
            Container.Bind<Configs>().FromInstance(mainConfig).AsSingle();

        }
    }
}

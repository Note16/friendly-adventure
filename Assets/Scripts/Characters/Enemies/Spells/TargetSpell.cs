namespace Assets.Scripts.Characters.Player.Attacks
{
    public class TargetSpell : BaseSpell
    {
        protected override void Awake()
        {
            base.Awake();

            StartCoroutine(HitEnemyAfterDelay(0.5f));
            StartCoroutine(HitEnemyAfterDelay(0.6f));
        }

        private void FixedUpdate()
        {
            Move(playerController.transform.position);
        }
    }
}
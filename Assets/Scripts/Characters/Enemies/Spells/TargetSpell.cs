namespace Assets.Scripts.Characters.Player.Attacks
{
    public class TargetSpell : BaseSpell
    {
        private float hitRange = 2f;

        protected override void Awake()
        {
            base.Awake();

            StartCoroutine(HitEnemyAfterDelay(0.5f, hitRange));
            StartCoroutine(HitEnemyAfterDelay(0.6f, hitRange));
        }

        private void FixedUpdate()
        {
            Move(playerController.transform.position);
        }
    }
}
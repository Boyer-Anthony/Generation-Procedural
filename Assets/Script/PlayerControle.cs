using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControle : MonoBehaviour

{
    // R�f�rences
    private CharacterController characterController;

    // Vitesse de d�placement
    public float speed = 5.0f;

    // Gravit� et saut
    public float gravity = -9.81f;
    public float jumpHeight = 2.0f;

    // Variables pour g�rer le mouvement
    private Vector3 velocity;
    private bool isGrounded;

    // Distance pour v�rifier si on touche le sol
    public float groundCheckDistance = 0.2f;
    public LayerMask groundMask; // Assurez-vous d'assigner le sol dans l'inspecteur

    void Start()
    {
        // R�cup�ration du CharacterController attach�
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // V�rifier si le personnage est au sol
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Maintient le personnage coll� au sol
        }

        // Obtenir les entr�es de d�placement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // D�terminer la direction de d�placement
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Appliquer le mouvement au CharacterController
        characterController.Move(move * speed * Time.deltaTime);

        // Gestion du saut
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("Jump");
        }

        // Appliquer la gravit�
        velocity.y += gravity * Time.deltaTime;

        // D�placer le personnage en fonction de la gravit�
        characterController.Move(velocity * Time.deltaTime);
    }

    // Affiche une sph�re dans l'�diteur pour voir la zone de d�tection du sol
    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, groundCheckDistance);
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControle : MonoBehaviour

{
    // Références
    private CharacterController characterController;

    // Vitesse de déplacement
    public float speed = 5.0f;

    // Gravité et saut
    public float gravity = -9.81f;
    public float jumpHeight = 2.0f;

    // Variables pour gérer le mouvement
    private Vector3 velocity;
    private bool isGrounded;

    // Distance pour vérifier si on touche le sol
    public float groundCheckDistance = 0.2f;
    public LayerMask groundMask; // Assurez-vous d'assigner le sol dans l'inspecteur

    void Start()
    {
        // Récupération du CharacterController attaché
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Vérifier si le personnage est au sol
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Maintient le personnage collé au sol
        }

        // Obtenir les entrées de déplacement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Déterminer la direction de déplacement
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Appliquer le mouvement au CharacterController
        characterController.Move(move * speed * Time.deltaTime);

        // Gestion du saut
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("Jump");
        }

        // Appliquer la gravité
        velocity.y += gravity * Time.deltaTime;

        // Déplacer le personnage en fonction de la gravité
        characterController.Move(velocity * Time.deltaTime);
    }

    // Affiche une sphère dans l'éditeur pour voir la zone de détection du sol
    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, groundCheckDistance);
    }*/
}
